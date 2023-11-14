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
        private readonly ISimulationService _simulationService;

        public WashingMachineService(IWashingMachineRepository washingMachineRepository, 
            IWashingMachineModeRepository washingMachineModeRepository,
            ISimulationService simulationService)
        {
            _washingMachineRepository = washingMachineRepository;
            _washingMachineModeRepository = washingMachineModeRepository;
            _simulationService = simulationService;
        }

        public async Task<WashingMachine> CreateWashingMachine(WashingMachine washingMachine)
        {
            washingMachine = await _washingMachineRepository.Create(washingMachine);
            await _simulationService.ToggleDeviceSimulator(washingMachine, true);
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
