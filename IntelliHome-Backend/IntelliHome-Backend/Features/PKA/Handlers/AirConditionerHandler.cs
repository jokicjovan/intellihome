using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Handlers
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
