using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace IntelliHome_Backend.Features.Communications.Services
{
    public class SmartDeviceConnectionService : ISmartDeviceConnectionService
    {
        private readonly IMqttService _mqttService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISimulationService _simulationService;
        public SmartDeviceConnectionService(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationService simulationService)
        {
            _mqttService = mqttService;
            _serviceProvider = serviceProvider;
            _simulationService = simulationService;
        }

        public Task<bool> ConnectWithSmartDevice(SmartDevice smartDevice) {
            return _simulationService.AddDeviceToSimulator(smartDevice);
        }

        public async Task SetupLastWillHandler()
        {
            await _mqttService.SubscribeAsync("will", HandleLastWillMessageAsync);
        }

        private async void HandleLastWillMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();
                Guid deviceId = Guid.Parse(e.ApplicationMessage.ConvertPayloadToString());
                SmartDevice smartDevice = await smartDeviceService.GetSmartDevice(deviceId);
                smartDevice.IsConnected = false;
                await smartDeviceService.UpdateSmartDevice(smartDevice);
            }
        }

    }
}
