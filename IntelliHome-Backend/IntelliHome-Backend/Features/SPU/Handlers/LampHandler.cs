using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.Home.Handlers;

namespace IntelliHome_Backend.Features.SPU.Handlers
{
    public class LampHandler : SmartDeviceHandler, ILampHandler
    {
        public LampHandler(IConfiguration configuration, MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(configuration, mqttFactory, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.SPU}/{SmartDeviceType.LAMP}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();
            
            var lampService = scope.ServiceProvider.GetRequiredService<ILampService>();
            
            var lamp = await lampService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));
            
            if (lamp != null)
            {
                var lampData = JsonConvert.DeserializeObject<LampData>(e.ApplicationMessage.ConvertPayloadToString() );
                var lampDataInflux = new Dictionary<string, object>
                {
                        { "currentBrightness", lampData.CurrentBrightness },
                        { "isShining", lampData.IsShining ? 1f : 0f },
                        { "isAuto", lampData.IsAuto ? 1f : 0f},
                        { "consumptionPerMinute", lampData.ConsumptionPerMinute }
            
                };
                var lampDataTags = new Dictionary<string, string>
                {
                        { "deviceId", lamp.Id.ToString() }
                };
                lampService.AddPoint(lampDataInflux, lampDataTags);
            }


        }

        public override Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            Lamp lamp = (Lamp)smartDevice;
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "brightness_limit", lamp.BrightnessLimit },
                            { "is_auto", lamp.IsAuto},
                            { "power_per_hour", lamp.PowerPerHour},
                        };
            var requestBody = new
            {
                device_id = smartDevice.Id,
                smart_home_id = smartDevice.SmartHome.Id,
                device_category = smartDevice.Category.ToString(),
                device_type = smartDevice.Type.ToString(),
                host = configuration["MqttBroker:Host"],
                port = configuration["MqttBroker:Port"],
                keepalive = configuration["MqttBroker:Keepalive"],
                kwargs = additionalAttributes
            };
            return simualtionsHandler.AddDeviceToSimulator(requestBody);
        }

        public void ChangeMode(Lamp lamp, bool isAuto)
        {
            string action = isAuto ? "auto" : "manual";
            string payload = JsonConvert.SerializeObject(new { action });
            PublishMessageToSmartDevice(lamp, payload);
        }

        public void ChangeBrightnessLimit(Lamp lamp, double brightness)
        {
            string action = $"set_brightness_limit";
            string payload = JsonConvert.SerializeObject(new { action, brightness });
            PublishMessageToSmartDevice(lamp, payload);
        }

        public void TurnLightOnOff(Lamp lamp, bool turnOn)
        {
            string action = turnOn ? "turn_lamp_on" : "turn_lamp_off";
            string payload = JsonConvert.SerializeObject(new { action });
            PublishMessageToSmartDevice(lamp, payload);
        }
    }
}
