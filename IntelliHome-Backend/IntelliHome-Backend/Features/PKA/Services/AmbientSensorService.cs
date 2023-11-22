using Data.Models.PKA;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AmbientSensorService : IAmbientSensorService
    {
        private readonly IAmbientSensorRepository _ambientSensorRepository;
        private readonly ISmartDeviceConnectionService _deviceConnectionService;

        public AmbientSensorService(IAmbientSensorRepository ambientSensorRepository, ISmartDeviceConnectionService deviceConnectionService)
        {
            _ambientSensorRepository = ambientSensorRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<AmbientSensor> CreateAmbientSensor(AmbientSensor ambientSensor)
        {
            ambientSensor = await _ambientSensorRepository.Create(ambientSensor);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(ambientSensor);
            if (success)
            {
                ambientSensor.IsConnected = true;
                await _ambientSensorRepository.Update(ambientSensor);
            }
            return ambientSensor;
        }
    }
}
