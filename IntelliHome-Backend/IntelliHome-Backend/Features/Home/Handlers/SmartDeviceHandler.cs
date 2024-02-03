using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MQTTnet.Client;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;
using Newtonsoft.Json;
using MQTTnet;
using IntelliHome_Backend.Features.Shared.Services;

namespace IntelliHome_Backend.Features.Home.Handlers
{
    public class SmartDeviceHandler : ISmartDeviceHandler
    {
        protected readonly ISimulationsHandler simualtionsHandler;
        protected readonly IServiceProvider serviceProvider;
        protected readonly IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext;
        protected readonly IMqttService mqttService;
        protected readonly IConfiguration configuration;

        public SmartDeviceHandler(IConfiguration configuration, MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
        {
            this.simualtionsHandler = simualtionsHandler;
            this.serviceProvider = serviceProvider;
            this.smartDeviceHubContext = smartDeviceHubContext;
            this.configuration = configuration;
            mqttService = new MqttService(mqttFactory);
            mqttService.ConnectAsync(configuration["MqttBroker:Host"], Convert.ToInt32(configuration["MqttBroker:Port"])).Wait();
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

        public virtual Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            var requestBody = new
            {
                device_id = smartDevice.Id,
                smart_home_id = smartDevice.SmartHome.Id,
                device_category = smartDevice.Category.ToString(),
                device_type = smartDevice.Type.ToString(),
                host = configuration["MqttBroker:Host"],
                port = configuration["MqttBroker:Port"],
                keepalive = configuration["MqttBroker:Keepalive"]
            };
            return simualtionsHandler.AddDeviceToSimulator(requestBody);
        }

        protected virtual Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
