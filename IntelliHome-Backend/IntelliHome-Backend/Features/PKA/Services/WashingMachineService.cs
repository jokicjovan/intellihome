using Data.Models.PKA;
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

        public Task<WashingMachine> CreateWashingMachine(WashingMachine washingMachine)
        {
            return _washingMachineRepository.Create(washingMachine);
        }
    }
}
