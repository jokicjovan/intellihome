using Data.Models.PKA;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IWashingMachineModeService
    {
        List<WashingMachineMode> GetWashingMachineModes(List<Guid> modesIds);
    }
}
