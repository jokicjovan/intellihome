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

        public async Task<BatterySystem> Create(BatterySystem entity)
        {
            entity = await _batterySystemRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _batterySystemRepository.Update(entity);
            //}
            return entity;
        }

        public Task<BatterySystem> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<BatterySystem> Get(Guid id)
        {
            BatterySystem batterySystem = await _batterySystemRepository.Read(id);
            if (batterySystem == null)
            {
                throw new ResourceNotFoundException("Battery system with provided Id not found!");
            }
            return batterySystem;
        }

        public Task<IEnumerable<BatterySystem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BatterySystem> Update(BatterySystem entity)
        {
            throw new NotImplementedException();
        }
    }
}
