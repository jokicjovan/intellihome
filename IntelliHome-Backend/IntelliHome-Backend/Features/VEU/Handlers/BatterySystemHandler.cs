using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.Handlers
{
    public class BatterySystemHandler : SmartDeviceHandler, IBatterySystemHandler
    {
        public BatterySystemHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttService, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            this.mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.VEU}/{SmartDeviceType.BATTERYSYSTEM}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"{e.ApplicationMessage.ConvertPayloadToString()}");
            _smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();

            var batterySystemService = scope.ServiceProvider.GetRequiredService<IBatterySystemService>();
            var batterySystem = await batterySystemService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));
            if (batterySystem != null)
            {
                var batterySystemData = JsonConvert.DeserializeObject<BatterySystemDTO>(e.ApplicationMessage.ConvertPayloadToString());
                var batterySystemDataInflux = new Dictionary<string, object>
                    {
                        { "current_capacity", batterySystemData.CurrentCapacity }
                    };
                var batterySystemDataTags = new Dictionary<string, string>
                    {
                        { "device_id", batterySystemData.Id.ToString() }
                    };
                batterySystemService.AddPoint(batterySystemDataInflux, batterySystemDataTags);
            }
        }
    }
}
