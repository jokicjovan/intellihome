using Data.Models.PKA;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AirConditionerService : IAirConditionerService
    {
        private readonly IAirConditionerRepository _airConditionerRepository;
        private readonly ISimulationService _simulationService;

        public AirConditionerService(IAirConditionerRepository airConditionerRepository, ISimulationService simulationService)
        {
            _airConditionerRepository = airConditionerRepository;
            _simulationService = simulationService;
        }

        public async Task<AirConditioner> CreateAirConditioner(AirConditioner airConditioner) {
            airConditioner = await _airConditionerRepository.Create(airConditioner);
            await _simulationService.ToggleDeviceSimulator(airConditioner, true);
            return airConditioner;
        }
    }
}
