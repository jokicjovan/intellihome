using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Handlers;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.PKA.Handlers
{
    public class AmbientSensorHandler : SmartDeviceHandler, IAmbientSensorHandler
    {
        public AmbientSensorHandler(MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext) 
            : base(mqttFactory, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.PKA}/{SmartDeviceType.AMBIENTSENSOR}/+", HandleMessageFromDevice);
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
                        { "deviceId", ambientSensor.Id.ToString() }
                    };
                ambientSensorService.AddPoint(ambientSensorDataInflux, ambientSensorDataTags);
            }
        }

        public override Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            AmbientSensor ambientSensor = (AmbientSensor)smartDevice;
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "power_per_hour", ambientSensor.PowerPerHour},
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