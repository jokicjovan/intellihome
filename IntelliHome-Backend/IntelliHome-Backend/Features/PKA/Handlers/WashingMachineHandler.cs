using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using IntelliHome_Backend.Features.Home.Handlers;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services;
using Newtonsoft.Json;
using System.Globalization;

namespace IntelliHome_Backend.Features.PKA.Handlers
{
    public class WashingMachineHandler : SmartDeviceHandler, IWashingMachineHandler
    {
        public WashingMachineHandler(MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttFactory, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.PKA}/{SmartDeviceType.WASHINGMACHINE}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());
            using (var scope = serviceProvider.CreateScope())
            {
                var washingMachineService = scope.ServiceProvider.GetRequiredService<IWashingMachineService>();

                var washingMachine = await washingMachineService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));

                if (washingMachine != null)
                {
                    var washingMachineData = JsonConvert.DeserializeObject<WashingMachineData>(e.ApplicationMessage.ConvertPayloadToString());
                    var washingMachineDataInflux = new Dictionary<string, object>
                    {
                        { "temperature", washingMachineData.Temperature},
                    };
                    var washingMachineDataTags = new Dictionary<string, string>
                    {
                        { "deviceId", washingMachine.Id.ToString() },
                        { "mode", washingMachineData.Mode},
                    };
                    washingMachineService.AddPoint(washingMachineDataInflux, washingMachineDataTags);
                }
            }
        }

        public override Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            var scope = serviceProvider.CreateScope();
            var washingMachineService = scope.ServiceProvider.GetRequiredService<IWashingMachineService>();
            WashingMachine washingMachine = washingMachineService.GetWithHome(smartDevice.Id).Result;
            var result = washingMachine.ScheduledWorks
                .SelectMany(work => work.DateTo != null
                    ? new[]
                    {
                                    new { timestamp = $"{work.DateFrom.ToString("dd/MM/yyyy")} {work.Start.ToString("HH:mm")}", mode = work.Mode.ToString().ToLower(), temperature = work.Temperature },
                                    new { timestamp = $"{work.DateTo.ToString("dd/MM/yyyy")} {work.End.ToString("HH:mm")}", mode = "turn_off", temperature = work.Temperature }
                    }
                    : new[]
                    {
                                    new { timestamp = $"{work.DateFrom} {work.Start}", mode = work.Mode.ToString().ToLower(), temperature = work.Temperature }
                    })
                .ToList().Where(work => DateTime.ParseExact(work.timestamp, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) > DateTime.UtcNow).ToList();
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            {"power_per_hour", washingMachine.PowerPerHour},
                            {"schedule_list", result }
                        };
            var requestBody = new
            {
                device_id = smartDevice.Id,
                smart_home_id = smartDevice.SmartHome.Id,
                device_category = smartDevice.Category.ToString(),
                device_type = smartDevice.Type.ToString(),
                host = "localhost",
                port = 1883,
                keepalive = 30,
                kwargs = additionalAttributes
            };
            return simualtionsHandler.AddDeviceToSimulator(requestBody);
        }


        public void ChangeMode(WashingMachine washingMachine, string mode,double temperature)
        {
            String modeLower = mode.ToLower();
            string action = $"{modeLower}";
            string payload = JsonConvert.SerializeObject(new { action, temperature });
            PublishMessageToSmartDevice(washingMachine, payload);
        }

        public void AddSchedule(WashingMachine washingMachine, string timestamp, string mode, double temperature)
        {
            string action = $"add_schedule";
            string payload = JsonConvert.SerializeObject(new { action, timestamp, mode, temperature });
            PublishMessageToSmartDevice(washingMachine, payload);
        }

    }
}
