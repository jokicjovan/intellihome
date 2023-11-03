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
    }
}
