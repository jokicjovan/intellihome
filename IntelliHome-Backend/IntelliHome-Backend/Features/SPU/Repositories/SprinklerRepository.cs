using Data.Context;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.SPU.Repositories
{
    public class SprinklerRepository : CrudRepository<Sprinkler>, ISprinklerRepository
    {
        public SprinklerRepository(PostgreSqlDbContext context) : base(context) { }
        public Task<Sprinkler> ReadWithSmartHome(Guid id)
        {
            return _entities.Include(s => s.SmartHome).Include(s => s.ScheduledWorks).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
