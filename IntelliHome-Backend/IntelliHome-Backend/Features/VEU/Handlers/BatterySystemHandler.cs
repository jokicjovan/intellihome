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
using IntelliHome_Backend.Features.Home.Handlers;
using Data.Models.VEU;
using IntelliHome_Backend.Features.VEU.DTOs.BatterySystem;

namespace IntelliHome_Backend.Features.VEU.Handlers
{
    public class BatterySystemHandler : SmartDeviceHandler, IBatterySystemHandler
    {
        public BatterySystemHandler(MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, 
            IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttFactory, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.VEU}/{SmartDeviceType.BATTERYSYSTEM}/+", HandleMessageFromDevice);
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
            var batterySystemData = JsonConvert.DeserializeObject<BatterySystemCapacityDataDTO>(e.ApplicationMessage.ConvertPayloadToString());
            if (batterySystem != null && batterySystemData != null)
            {
                var fields = new Dictionary<string, object>
                    {
                        { "currentCapacity", batterySystemData.CurrentCapacity }
                    };
                var tags = new Dictionary<string, string>
                    {
                        { "deviceId", batterySystem.Id.ToString() }
                    };
                batterySystemService.AddCapacityMeasurement(fields, tags);
            }
        }

        public override Task<bool> ConnectToSmartDevice(SmartDevice smartDevice) {
            BatterySystem batterySystem = (BatterySystem)smartDevice;
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "capacity", batterySystem.Capacity }
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
