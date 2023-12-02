using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace IntelliHome_Backend.Features.PKA.Handlers
{
    public class AmbientSensorHandler : IAmbientSensorHandler
    {
        private readonly ISimulationsHandler _simualtionsHandler;
        private readonly IMqttService _mqttService;
        private readonly IServiceProvider _serviceProvider;

        public AmbientSensorHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler)
        {
            _simualtionsHandler = simualtionsHandler;
            _mqttService = mqttService;
            _serviceProvider = serviceProvider;
        }

        public void SubscribeToAmbientSensorsFromDatabase()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IAmbientSensorService ambientSensorService = scope.ServiceProvider.GetRequiredService<IAmbientSensorService>();
                IEnumerable<AmbientSensor> ambientSensors = ambientSensorService.GetAllWithHome();
                foreach (SmartDevice ambientSensor in ambientSensors)
                {
                    SubscribeToAmbientSensor($"FromDevice/{ambientSensor.SmartHome.Id}/{ambientSensor.Category}/{ambientSensor.Type}/{ambientSensor.Id}");
                }
            }
        }

        private Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            return Task.CompletedTask;
        }

        public async void SubscribeToAmbientSensor(string topic)
        {
            await _mqttService.SubscribeAsync(topic, HandleMessageFromDevice);
        }

        public async void PublishMessageToAmbientSensor(AmbientSensor ambientSensor, string payload)
        {
            string topic = $"ToDevice/{ambientSensor.SmartHome.Id}/{ambientSensor.Category}/{ambientSensor.Type}/{ambientSensor.Id}";
            await _mqttService.PublishAsync(topic, payload);
        }

        public async Task<bool> AddAmbientSensorToSimulator(AmbientSensor ambientSensor)
        {
            var requestBody = new
            {
                device_id = ambientSensor.Id,
                smart_home_id = ambientSensor.SmartHome.Id,
                device_category = ambientSensor.Category.ToString(),
                device_type = ambientSensor.Type.ToString(),
                host = "localhost",
                port = 1883,
                keepalive = 30
            };
            return await _simualtionsHandler.AddDeviceToSimulator(requestBody);
        }

    }
}
