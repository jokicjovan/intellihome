using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class WashingMachineService : IWashingMachineService
    {
        private readonly IWashingMachineRepository _washingMachineRepository;
        private readonly IWashingMachineModeRepository _washingMachineModeRepository;

        public WashingMachineService(IWashingMachineRepository washingMachineRepository, IWashingMachineModeRepository washingMachineModeRepository)
        {
            _washingMachineRepository = washingMachineRepository;
            _washingMachineModeRepository = washingMachineModeRepository;
        }

        public Task<WashingMachine> CreateWashingMachine(WashingMachine washingMachine)
        {
            return _washingMachineRepository.Create(washingMachine);
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
