using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace IntelliHome_Backend.Features.PKA.Handlers
{
    public class AmbientSensorHandler : SmartDeviceHandler, IAmbientSensorHandler
    {
        public AmbientSensorHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler) 
            : base(mqttService, serviceProvider, simualtionsHandler)
        {

        }

        protected override Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            return Task.CompletedTask;
        }
    }
}
