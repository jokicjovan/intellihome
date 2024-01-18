using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class WashingMachineService : IWashingMachineService
    {
        private readonly IWashingMachineRepository _washingMachineRepository;
        private readonly IWashingMachineModeRepository _washingMachineModeRepository;
        private readonly IWashingMachineHandler _washingMachineHandler;

        public WashingMachineService(IWashingMachineRepository washingMachineRepository, 
            IWashingMachineModeRepository washingMachineModeRepository, IWashingMachineHandler washingMachineHandler)
        {
            _washingMachineRepository = washingMachineRepository;
            _washingMachineModeRepository = washingMachineModeRepository;
            _washingMachineHandler = washingMachineHandler;
        }

        public List<WashingMachineMode> GetWashingMachineModes(List<Guid> modesIds)
        {
            return _washingMachineModeRepository.FindWashingMachineModes(modesIds);
        }

        public Task<IEnumerable<WashingMachineMode>> GetAllWashingMachineModes()
        {
            return _washingMachineModeRepository.ReadAll();
        }

        public async Task<WashingMachine> Create(WashingMachine entity)
        {
            entity = await _washingMachineRepository.Create(entity);
            bool success = await _washingMachineHandler.ConnectToSmartDevice(entity);
            if (success)
            {
                entity.IsConnected = true;
                await _washingMachineRepository.Update(entity);
            }
            return entity;
        }

        public Task<WashingMachine> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WashingMachine>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<WashingMachine> Update(WashingMachine entity)
        {
            throw new NotImplementedException();
        }

        public Task<WashingMachine> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
