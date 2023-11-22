using Data.Models.VEU;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class BatterySystemService : IBatterySystemService
    {
        private readonly IBatterySystemRepository _batterySystemRepository;
        private readonly ISmartDeviceConnectionService _deviceConnectionService;

        public BatterySystemService(IBatterySystemRepository batterySystemRepository, ISmartDeviceConnectionService deviceConnectionService)
        {
            _batterySystemRepository = batterySystemRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<BatterySystem> CreateBatterySystem(BatterySystem batterySystem)
        {
            batterySystem = await _batterySystemRepository.Create(batterySystem);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(batterySystem);
            if (success)
            {
                batterySystem.IsConnected = true;
                await _batterySystemRepository.Update(batterySystem);
            }
            return batterySystem;
        }
        
        public async Task<BatterySystem> GetBatterySystem(Guid Id)
        {
            BatterySystem batterySystem = await _batterySystemRepository.Read(Id);
            if (batterySystem == null)
            {
                throw new ResourceNotFoundException("Battery system with provided Id not found!");
            }
            return batterySystem;
        }
    }
}
