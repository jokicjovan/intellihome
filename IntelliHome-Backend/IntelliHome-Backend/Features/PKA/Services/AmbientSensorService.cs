using Data.Models.PKA;
using IntelliHome_Backend.Features.Communications.Services;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AmbientSensorService : IAmbientSensorService
    {
        private readonly IAmbientSensorRepository _ambientSensorRepository;
        private readonly ISimulationService _simulationService;

        public AmbientSensorService(IAmbientSensorRepository ambientSensorRepository, ISimulationService simulationService)
        {
            _ambientSensorRepository = ambientSensorRepository;
            _simulationService = simulationService;
        }

        public async Task<AmbientSensor> CreateAmbientSensor(AmbientSensor ambientSensor)
        {
            ambientSensor = await _ambientSensorRepository.Create(ambientSensor);
            await _simulationService.ToggleDeviceSimulator(ambientSensor, true);
            return ambientSensor;
        }
    }
}
