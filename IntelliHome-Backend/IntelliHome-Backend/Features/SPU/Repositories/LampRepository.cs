using Data.Context;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.SPU.Repositories
{
    public class LampRepository : CrudRepository<Lamp>, ILampRepository
    {
        public LampRepository(PostgreSqlDbContext context) : base(context) { }
        public async Task<Lamp> GetWithSmartHome(Guid id)
        {
            return await _entities.Include(l => l.SmartHome).FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
