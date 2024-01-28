using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Repositories.Interfaces
{
    public interface ISprinklerRepository : ICrudRepository<Sprinkler>
    {
        Task<Sprinkler> ReadWithSmartHome(Guid id);
    }
}
