using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces;
using IntelliHome_Backend.Features.Communications.Handlers.PKA.Interfaces;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace IntelliHome_Backend.Features.Communications.Handlers.PKA
{
    public class AmbientSensorHandler : IAmbientSensorHandler
    {
        private readonly ISimulationsHandler _simulationsHandler;
        private readonly IMqttService _mqttService;
        private readonly IServiceProvider _serviceProvider;

        public AmbientSensorHandler(ISimulationsHandler simulationsHandler, IMqttService mqttService, IServiceProvider serviceProvider)
        {
            _simulationsHandler = simulationsHandler;
            _mqttService = mqttService;
            _serviceProvider = serviceProvider;
        }

        public async void RegisterAmbientSensorListeners() {
            using (var scope = _serviceProvider.CreateScope())
            {
                IAmbientSensorService ambientSensorService = scope.ServiceProvider.GetRequiredService<IAmbientSensorService>();
                IEnumerable<AmbientSensor> ambientSensors = ambientSensorService.GetAllWithHome();
                foreach (SmartDevice ambientSensor in ambientSensors)
                {
                    await _mqttService.SubscribeAsync($"FromDevice/{ambientSensor.SmartHome.Id}/{ambientSensor.Category}/{ambientSensor.Type}/{ambientSensor.Id}", HandleMessageFromDevice);
                }
            }
        }

        private async void HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
        }
    }
}
