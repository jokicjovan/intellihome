using Data.Context;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.PKA.Repositories
{
    public class WashingMachineRepository : CrudRepository<WashingMachine>, IWashingMachineRepository
    {
        public WashingMachineRepository(PostgreSqlDbContext context) : base(context) { }

        public IEnumerable<WashingMachine> FindAllWIthHome()
        {
            return _entities.Include(p => p.SmartHome).Include(p => p.ScheduledWorks).Include(p=>p.Modes);
        }
        public Task<WashingMachine> FindWIthHome(Guid id)
        {
            return _entities.Include(p => p.SmartHome).Include(p => p.ScheduledWorks).Include(p => p.Modes).FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<WashingMachine> FindWithSmartHome(Guid id)
        {
            return _entities.Include(e => e.SmartHome).Include(p => p.Modes).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
