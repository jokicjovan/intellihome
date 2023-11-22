using Data.Models.PKA;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class WashingMachineService : IWashingMachineService
    {
        private readonly IWashingMachineRepository _washingMachineRepository;
        private readonly IWashingMachineModeRepository _washingMachineModeRepository;
        private readonly ISmartDeviceConnectionService _deviceConnectionService;

        public WashingMachineService(IWashingMachineRepository washingMachineRepository, 
            IWashingMachineModeRepository washingMachineModeRepository,
            ISmartDeviceConnectionService deviceConnectionService)
        {
            _washingMachineRepository = washingMachineRepository;
            _washingMachineModeRepository = washingMachineModeRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<WashingMachine> CreateWashingMachine(WashingMachine washingMachine)
        {
            washingMachine = await _washingMachineRepository.Create(washingMachine);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(washingMachine);
            if (success)
            {
                washingMachine.IsConnected = true;
                await _washingMachineRepository.Update(washingMachine);
            }
            return washingMachine;
        }

        public List<WashingMachineMode> GetWashingMachineModes(List<Guid> modesIds)
        {
            return _washingMachineModeRepository.FindWashingMachineModes(modesIds);
        }

        public Task<IEnumerable<WashingMachineMode>> GetAllWashingMachineModes()
        {
            return _washingMachineModeRepository.ReadAll();
        }
    }
}
