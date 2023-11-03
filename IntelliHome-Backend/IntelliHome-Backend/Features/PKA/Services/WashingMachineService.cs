using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class WashingMachineService : IWashingMachineService
    {
        private readonly IWashingMachineRepository _washingMachineRepository;

        public WashingMachineService(IWashingMachineRepository washingMachineRepository)
        {
            _washingMachineRepository = washingMachineRepository;
        }
    }
}
