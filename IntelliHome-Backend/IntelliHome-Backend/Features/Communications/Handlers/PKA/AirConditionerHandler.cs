using IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;

namespace IntelliHome_Backend.Features.Communications.Handlers.PKA
{
    public class AirConditionerHandler
    {
        private readonly ISimulationsHandler _simulationsHandler;
        private readonly IMqttService _mqttService;

        public AirConditionerHandler(ISimulationsHandler simulationsHandler, IMqttService mqttService)
        {
            _simulationsHandler = simulationsHandler;
            _mqttService = mqttService;
        }


    }
}
