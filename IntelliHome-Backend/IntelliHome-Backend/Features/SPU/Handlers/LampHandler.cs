using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;

namespace IntelliHome_Backend.Features.SPU.Handlers
{
    public class LampHandler : SmartDeviceHandler, ILampHandler
    {
        public LampHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler)
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
