using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MQTTnet.Client;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;
using Data.Models.VEU;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.Home.Handlers
{
    public class SmartDeviceHandler : ISmartDeviceHandler
    {
        protected readonly ISimulationsHandler simualtionsHandler;
        protected readonly IMqttService mqttService;
        protected readonly IServiceProvider serviceProvider;
        protected readonly IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext;

        public SmartDeviceHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
        {
            this.simualtionsHandler = simualtionsHandler;
            this.mqttService = mqttService;
            this.serviceProvider = serviceProvider;
            this.smartDeviceHubContext = smartDeviceHubContext;
        }

        public Task SubscribeToSmartDevice(SmartDevice smartDevice)
        {
            string topic = $"FromDevice/{smartDevice.SmartHome.Id}/{smartDevice.Category}/{smartDevice.Type}/{smartDevice.Id}";
            return mqttService.SubscribeAsync(topic, HandleMessageFromDevice);
        }

        public virtual Task ToggleSmartDevice(SmartDevice smartDevice, bool turnOn)
        {
            smartDeviceHubContext.Clients.Group(smartDevice.Id.ToString()).ReceiveSmartDeviceData(JsonConvert.SerializeObject(new { isOn = turnOn }));
            string action = turnOn ? "turn_on" : "turn_off";
            string payload = $"{{\"action\": \"{action}\"}}";
            return PublishMessageToSmartDevice(smartDevice, payload);
        }

        public Task PublishMessageToSmartDevice(SmartDevice smartDevice, string payload)
        {
            string topic = $"ToDevice/{smartDevice.SmartHome.Id}/{smartDevice.Category}/{smartDevice.Type}/{smartDevice.Id}";
            return mqttService.PublishAsync(topic, payload);
        }

        public Task<bool> ConnectToSmartDevice(SmartDevice smartDevice, Dictionary<string, object> additionalAttributes)
        {
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

        protected virtual Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
