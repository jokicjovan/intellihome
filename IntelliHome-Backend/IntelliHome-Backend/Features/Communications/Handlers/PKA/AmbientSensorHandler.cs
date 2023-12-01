using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Handlers.PKA.Interfaces;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace IntelliHome_Backend.Features.Communications.Handlers.PKA
{
    public class AmbientSensorHandler : IAmbientSensorHandler
    {
        private readonly IMqttService _mqttService;
        private readonly IServiceProvider _serviceProvider;

        public AmbientSensorHandler(IMqttService mqttService, IServiceProvider serviceProvider)
        {
            _mqttService = mqttService;
            _serviceProvider = serviceProvider;
        }

        public async void RegisterAmbientSensorsListeners() {
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

        private Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            return Task.CompletedTask;
        }

        public async void PublishMessageToAmbientSensor(AmbientSensor ambientSensor, String payload)
        {
            String topic = $"ToDevice/{ambientSensor.SmartHome.Id}/{ambientSensor.Category}/{ambientSensor.Type}/{ambientSensor.Id}";
            await _mqttService.PublishAsync(topic, payload);
        }
    }
}
