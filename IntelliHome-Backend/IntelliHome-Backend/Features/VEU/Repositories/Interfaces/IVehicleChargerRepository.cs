using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Repositories.Interfaces
{
    public interface IVehicleChargerRepository : ICrudRepository<VehicleCharger>
    {
        Task<VehicleCharger> FindWithSmartHome(Guid id);
    }
}
