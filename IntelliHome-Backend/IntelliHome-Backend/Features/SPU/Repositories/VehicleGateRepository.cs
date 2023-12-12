using Data.Context;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.SPU.Repositories
{
    public class VehicleGateRepository : CrudRepository<VehicleGate>, IVehicleGateRepository
    {
        public VehicleGateRepository(PostgreSqlDbContext context) : base(context) { }
        public Task<VehicleGate> FindWithSmartHome(Guid id)
        {
            return _entities.Include(x => x.SmartHome).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
