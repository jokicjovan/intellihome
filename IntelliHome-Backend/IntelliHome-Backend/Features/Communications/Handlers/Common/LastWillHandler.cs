using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace IntelliHome_Backend.Features.Communications.Handlers.Common
{
    public class LastWillHandler : ILastWillHandler
    {
        private readonly IMqttService _mqttService;
        private readonly IServiceProvider _serviceProvider;
        public LastWillHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simulationService)
        {
            _mqttService = mqttService;
            _serviceProvider = serviceProvider;
        }

        public async Task SetupLastWillHandler()
        {
            await _mqttService.SubscribeAsync("will", HandleLastWillMessageAsync);
        }

        private async Task HandleLastWillMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();
                Guid deviceId = Guid.Parse(e.ApplicationMessage.ConvertPayloadToString());
                SmartDevice smartDevice = await smartDeviceService.Get(deviceId);
                smartDevice.IsConnected = false;
                await smartDeviceService.Update(smartDevice);
            }
        }

    }
}
