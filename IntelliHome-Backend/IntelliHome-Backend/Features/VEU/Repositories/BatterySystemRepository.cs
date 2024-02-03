using Data.Context;
using Data.Models.SPU;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.VEU.Repositories
{
    public class BatterySystemRepository : CrudRepository<BatterySystem>, IBatterySystemRepository
    {
        public BatterySystemRepository(PostgreSqlDbContext context) : base(context) { }

        public Task<BatterySystem> FindWithSmartHome(Guid id)
        {
            return _entities.Include(e => e.SmartHome).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
