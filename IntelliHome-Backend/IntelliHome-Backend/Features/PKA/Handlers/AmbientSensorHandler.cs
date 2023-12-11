using Data.Context;
using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Handlers;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.PKA.Handlers
{
    public class AmbientSensorHandler : SmartDeviceHandler, IAmbientSensorHandler
    {
        public AmbientSensorHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext) 
            : base(mqttService, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            this.mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.PKA}/{SmartDeviceType.AMBIENTSENSOR}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"{e.ApplicationMessage.ConvertPayloadToString()}");
            smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();

            var ambientSensorService = scope.ServiceProvider.GetRequiredService<IAmbientSensorService>();

            var ambientSensor = await ambientSensorService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));

            if (ambientSensor != null)
            {
                var ambientSensorData = JsonConvert.DeserializeObject<AmbientSensorData>(e.ApplicationMessage.ConvertPayloadToString());
                var ambientSensorDataInflux = new Dictionary<string, object>
                    {
                        { "temperature", ambientSensorData.Temperature },
                        { "humidity", ambientSensorData.Humidity },
                    };
                var ambientSensorDataTags = new Dictionary<string, string>
                    {
                        { "device_id", ambientSensor.Id.ToString() }
                    };
                ambientSensorService.AddPoint(ambientSensorDataInflux, ambientSensorDataTags);
            }
        }
    }
}