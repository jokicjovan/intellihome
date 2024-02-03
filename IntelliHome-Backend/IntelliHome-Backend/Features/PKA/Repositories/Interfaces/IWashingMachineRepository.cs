using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Repositories.Interfaces
{
    public interface IWashingMachineRepository : ICrudRepository<WashingMachine>
    {
        IEnumerable<WashingMachine> FindAllWIthHome();
        Task<WashingMachine> FindWIthHome(Guid id);
        Task<WashingMachine> FindWithSmartHome(Guid id);
    }
}
