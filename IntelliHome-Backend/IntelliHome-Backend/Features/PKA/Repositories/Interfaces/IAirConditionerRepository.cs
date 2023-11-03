using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Repositories.Interfaces
{
    public interface IAirConditionerRepository : ICrudRepository<AirConditioner>
    {
    }
}