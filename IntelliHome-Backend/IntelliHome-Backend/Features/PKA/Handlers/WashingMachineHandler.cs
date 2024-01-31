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

            using (var scope = serviceProvider.CreateScope())
            {
                var washingMachineService = scope.ServiceProvider.GetRequiredService<IWashingMachineService>();

                var washingMachine = await washingMachineService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));

                if (washingMachine != null)
                {
                    //TODO: Handle message from device
                }
            }
        }

        public override Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            WashingMachine washingMachine = (WashingMachine)smartDevice;
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "power_per_hour", washingMachine.PowerPerHour},
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
    }
}
