using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.SPU.Handlers
{
    public class LampHandler : SmartDeviceHandler, ILampHandler
    {
        public LampHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttService, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            this.mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.SPU}/{SmartDeviceType.LAMP}/+", HandleMessageFromDevice);
        }

        protected override Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();

            var lampService = scope.ServiceProvider.GetRequiredService<ILampService>();

            var lamp = lampService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));

            if (lamp != null)
            {
                var lampData = JsonConvert.DeserializeObject<LampData>(e.ApplicationMessage.ConvertPayloadToString());
                var lampDataInflux = new Dictionary<string, object>
                {
                        { "currentBrightness", lampData.CurrentBrightness }
                };
                var lampDataTags = new Dictionary<string, string>
                {
                        { "deviceId", lamp.Id.ToString() }
                };
                lampService.AddPoint(lampDataInflux, lampDataTags);
            }

            
            
            return Task.CompletedTask;
        }
    }
}
