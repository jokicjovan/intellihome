using Data.Context;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.SPU.Repositories
{
    public class SprinklerWorkRepository : CrudRepository<SprinklerWork>, ISprinklerWorkRepository
    {
        public SprinklerWorkRepository(PostgreSqlDbContext context) : base(context)
        {
        }
    }
}
