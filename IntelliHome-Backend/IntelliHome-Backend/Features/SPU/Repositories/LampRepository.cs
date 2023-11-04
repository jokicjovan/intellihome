using Data.Context;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Repositories
{
    public class LampRepository : CrudRepository<Lamp>, ILampRepository
    {
        public LampRepository(PostgreSqlDbContext context) : base(context) { }
    }
}
