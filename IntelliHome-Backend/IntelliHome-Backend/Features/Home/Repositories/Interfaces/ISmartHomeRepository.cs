using Data.Models.Home;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.Home.Repositories.Interfaces
{
    public interface ISmartHomeRepository : ICrudRepository<SmartHome>
    {
    }
}
