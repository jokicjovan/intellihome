using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using MQTTnet.Client;

namespace IntelliHome_Backend.Features.Shared.Handlers
{
    public class SmartDeviceHandler : ISmartDeviceHandler
    {
        protected readonly ISimulationsHandler simualtionsHandler;
        protected readonly IMqttService mqttService;
        protected readonly IServiceProvider serviceProvider;

        public SmartDeviceHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler)
        {
            this.simualtionsHandler = simualtionsHandler;
            this.mqttService = mqttService;
            this.serviceProvider = serviceProvider;
        }

        public async void SubscribeToSmartDevice(SmartDevice smartDevice)
        {
            String topic = $"FromDevice/{smartDevice.SmartHome.Id}/{smartDevice.Category}/{smartDevice.Type}/{smartDevice.Id}";
            await mqttService.SubscribeAsync(topic, HandleMessageFromDevice);
        }

        public async void PublishMessageToSmartDevice(SmartDevice smartDevice, string payload)
        {
            string topic = $"ToDevice/{smartDevice.SmartHome.Id}/{smartDevice.Category}/{smartDevice.Type}/{smartDevice.Id}";
            await mqttService.PublishAsync(topic, payload);
        }

        public async Task<bool> AddSmartDeviceToSimulator(SmartDevice smartDevice, Dictionary<String, object> additionalAttributes)
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
            return await simualtionsHandler.AddDeviceToSimulator(requestBody);
        }

        protected virtual Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
