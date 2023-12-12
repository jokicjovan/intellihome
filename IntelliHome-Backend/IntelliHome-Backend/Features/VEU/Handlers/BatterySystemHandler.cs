using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.Home.Handlers;

namespace IntelliHome_Backend.Features.VEU.Handlers
{
    public class BatterySystemHandler : SmartDeviceHandler, IBatterySystemHandler
    {
        public BatterySystemHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, 
            IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttService, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            this.mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.VEU}/{SmartDeviceType.BATTERYSYSTEM}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            String[] topic_parts = e.ApplicationMessage.Topic.Split('/');
            if (topic_parts.Length < 5 )
            {
                Console.WriteLine("Error handling topic");
                return;
            }
            string batterySystemId = topic_parts.Last();
            _ = smartDeviceHubContext.Clients.Group(batterySystemId).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();
            var batterySystemService = scope.ServiceProvider.GetRequiredService<IBatterySystemService>();
            var batterySystem = await batterySystemService.Get(Guid.Parse(batterySystemId));
            if (batterySystem != null)
            {
                var batterySystemData = JsonConvert.DeserializeObject<BatterySystemCapacityDataDTO>(e.ApplicationMessage.ConvertPayloadToString());
                var batterySystemDataInflux = new Dictionary<string, object>
                    {
                        { "currentCapacity", batterySystemData.CurrentCapacity }
                    };
                var batterySystemDataTags = new Dictionary<string, string>
                    {
                        { "deviceId", batterySystem.Id.ToString() }
                    };
                batterySystemService.AddCapacityMeasurement(batterySystemDataInflux, batterySystemDataTags);
            }
        }
    }
}
