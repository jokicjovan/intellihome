using Data.Models.PKA;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IWashingMachineService
    {
        Task<WashingMachine> CreateWashingMachine(WashingMachine washingMachine);
    }
}
