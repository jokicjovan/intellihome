using Data.Context;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.VEU.Repositories
{
    public class VehicleChargerRepository : CrudRepository<VehicleCharger>, IVehicleChargerRepository
    {
        public VehicleChargerRepository(PostgreSqlDbContext context) : base(context) { }

        public Task<VehicleCharger> FindWithSmartHome(Guid id)
        {
            return _entities.Include(e => e.ChargingPoints).Include(e => e.SmartHome).FirstOrDefaultAsync(e => e.Id == id);
        }

        public override Task<VehicleCharger> Read(Guid id) {
            return _entities.Include(e => e.ChargingPoints).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
