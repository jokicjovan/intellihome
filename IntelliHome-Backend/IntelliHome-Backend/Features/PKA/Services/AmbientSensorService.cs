using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AmbientSensorService : IAmbientSensorService
    {
        private readonly IAmbientSensorRepository _ambientSensorRepository;

        public AmbientSensorService(IAmbientSensorRepository ambientSensorRepository)
        {
            _ambientSensorRepository = ambientSensorRepository;
        }

        public Task<AmbientSensor> CreateAmbientSensor(AmbientSensor ambientSensor)
        {
            return _ambientSensorRepository.Create(ambientSensor);
        }
    }
}
