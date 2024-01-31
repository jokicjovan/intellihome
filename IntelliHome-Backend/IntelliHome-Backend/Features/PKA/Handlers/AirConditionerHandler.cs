using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.Home.Handlers;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.DTOs;
using System.Globalization;

namespace IntelliHome_Backend.Features.PKA.Handlers
{
    public class AirConditionerHandler : SmartDeviceHandler, IAirConditionerHandler
    {
        public AirConditionerHandler(MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttFactory, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.PKA}/{SmartDeviceType.AIRCONDITIONER}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());


            using (var scope = serviceProvider.CreateScope())
            {
                var airConditionerService = scope.ServiceProvider.GetRequiredService<IAirConditionerService>();

                var airConditioner = await airConditionerService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));

                if (airConditioner != null)
                {
                    var airConditionerData = JsonConvert.DeserializeObject<AirConditionerData>(e.ApplicationMessage.ConvertPayloadToString());
                    var airConditionerDataInflux = new Dictionary<string, object>
                    {
                        { "temperature", airConditionerData.Temperature},
                    };
                    var airConditionerDataTags = new Dictionary<string, string>
                    {
                        { "deviceId", airConditioner.Id.ToString() },
                        { "mode", airConditionerData.Mode},
                    };
                    airConditionerService.AddPoint(airConditionerDataInflux, airConditionerDataTags);
                }


            }
        }

        public override Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            var scope = serviceProvider.CreateScope();
            var airConditionerService = scope.ServiceProvider.GetRequiredService<IAirConditionerService>();
            AirConditioner airConditioner = airConditionerService.GetWithHome(smartDevice.Id).Result;
            var result = airConditioner.ScheduledWorks
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
                            {"power_per_hour", airConditioner.PowerPerHour},
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

        public void ChangeTemperature(AirConditioner airConditioner, double temperature)
        {
            string action = $"set_current_temperature";
            string payload = JsonConvert.SerializeObject(new { action, temperature });
            PublishMessageToSmartDevice(airConditioner, payload);
        }

        public void ChangeMode(AirConditioner airConditioner, string mode)
        {
            string action = $"{mode}";
            string payload = JsonConvert.SerializeObject(new { action });
            PublishMessageToSmartDevice(airConditioner, payload);
        }

        public void AddSchedule(AirConditioner airConditioner,string timestamp,string mode, double temperature)
        {
            string action = $"add_schedule";
            string payload = JsonConvert.SerializeObject(new { action,timestamp,mode,temperature});
            PublishMessageToSmartDevice(airConditioner, payload);
        }
    }
}
