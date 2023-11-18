using Data.Models.PKA;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AirConditionerService : IAirConditionerService
    {
        private readonly IAirConditionerRepository _airConditionerRepository;
        private readonly IDeviceConnectionService _deviceConnectionService;

        public AirConditionerService(IAirConditionerRepository airConditionerRepository, IDeviceConnectionService deviceConnectionService)
        {
            _airConditionerRepository = airConditionerRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<AirConditioner> CreateAirConditioner(AirConditioner airConditioner) {
            airConditioner = await _airConditionerRepository.Create(airConditioner);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(airConditioner);
            if (success)
            {
                airConditioner.IsConnected = true;
                await _airConditionerRepository.Update(airConditioner);
            }
            return airConditioner;
        }
    }
}
