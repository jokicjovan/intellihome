using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.Repositories.Interfaces
{
    public interface ILampRepository : ICrudRepository<Lamp>
    {

        Task<Lamp> GetWithSmartHome(Guid id);
    }
}
