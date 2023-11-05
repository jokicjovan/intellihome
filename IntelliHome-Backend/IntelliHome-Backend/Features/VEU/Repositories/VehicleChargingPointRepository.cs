using Data.Context;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Repositories
{
    public class VehicleChargingPointRepository : CrudRepository<VehicleChargingPoint>, IVehicleChargingPointRepository
    {
        public VehicleChargingPointRepository(PostgreSqlDbContext context) : base(context) { }
    }
}
