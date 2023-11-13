using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class BatterySystemService : IBatterySystemService
    {
        private readonly IBatterySystemRepository _batterySystemRepository;

        public BatterySystemService(IBatterySystemRepository batterySystemRepository)
        {
            _batterySystemRepository = batterySystemRepository;
        }

        public Task<BatterySystem> CreateBatterySystem(BatterySystem batterySystem)
        {
            return _batterySystemRepository.Create(batterySystem);
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
