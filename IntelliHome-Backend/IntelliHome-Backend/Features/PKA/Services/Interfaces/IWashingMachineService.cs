using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IWashingMachineService : ICrudService<WashingMachine>
    {
        List<WashingMachineMode> GetWashingMachineModes(List<Guid> modesIds);
        Task<IEnumerable<WashingMachineMode>> GetAllWashingMachineModes();
    }
}
