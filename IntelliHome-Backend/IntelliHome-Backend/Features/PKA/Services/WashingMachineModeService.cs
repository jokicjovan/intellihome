using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class WashingMachineModeService : IWashingMachineModeService
    {
        private readonly IWashingMachineModeRepository _washingMachineModeRepository;

        public WashingMachineModeService(IWashingMachineModeRepository washingMachineModeRepository)
        {
            _washingMachineModeRepository = washingMachineModeRepository;
        }

        public List<WashingMachineMode> GetWashingMachineModes(List<Guid> modesIds)
        {
            return _washingMachineModeRepository.FindWashingMachineModes(modesIds);
        }
    }
}
