using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Repositories.Interfaces
{
    public interface IAirConditionerRepository : ICrudRepository<AirConditioner>
    {
        IEnumerable<AirConditioner> FindAllWIthHome();
        Task<AirConditioner> FindWIthHome(Guid id);
        Task<AirConditioner> FindWithSmartHome(Guid id);
    }
}