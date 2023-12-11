using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IntelliHome_Backend.Features.PKA.Handlers
{
    public class WashingMachineHandler : SmartDeviceHandler, IWashingMachineHandler
    {
        public WashingMachineHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttService, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            this.mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.PKA}/{SmartDeviceType.WASHINGMACHINE}/+", HandleMessageFromDevice);
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
    }
}
