using Data.Context;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Repositories
{
    public class SprinklerRepository : CrudRepository<Sprinkler>, ISprinklerRepository
    {
        public SprinklerRepository(PostgreSqlDbContext context) : base(context) { }
    }
}
