using Data.Models.PKA;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
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
        private readonly ISmartDeviceDataRepository _smartDeviceDataRepository;

        public WashingMachineService(IWashingMachineRepository washingMachineRepository, 
            IWashingMachineModeRepository washingMachineModeRepository, IWashingMachineHandler washingMachineHandler,
            ISmartDeviceDataRepository smartDeviceDataRepository)
        {
            _washingMachineRepository = washingMachineRepository;
            _washingMachineModeRepository = washingMachineModeRepository;
            _washingMachineHandler = washingMachineHandler;
            _smartDeviceDataRepository = smartDeviceDataRepository;
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
            if (!success) return entity;
            entity.IsConnected = true;
            await _washingMachineRepository.Update(entity);
            var fields = new Dictionary<string, object>
            {
                { "isConnected", true }

            };
            var tags = new Dictionary<string, string>
            {
                { "deviceId", entity.Id.ToString()}
            };
            _smartDeviceDataRepository.AddPoint(fields, tags);
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
