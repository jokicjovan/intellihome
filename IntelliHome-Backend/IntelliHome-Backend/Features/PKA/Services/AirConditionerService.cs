using Data.Models.PKA;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AirConditionerService : IAirConditionerService
    {
        private readonly IAirConditionerRepository _airConditionerRepository;
        private readonly IHeartbeatService _heartbeatService;

        public AirConditionerService(IAirConditionerRepository airConditionerRepository, IHeartbeatService heartbeatService)
        {
            _airConditionerRepository = airConditionerRepository;
            _heartbeatService = heartbeatService;
        }

        public async Task<AirConditioner> CreateAirConditioner(AirConditioner airConditioner) {
            airConditioner = await _airConditionerRepository.Create(airConditioner);
            await _heartbeatService.ToggleDeviceSimulator(airConditioner, true);
            await _heartbeatService.SetupLastWillHandler(airConditioner.Id);
            return airConditioner;
        }
    }
}
