using Data.Context;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;

namespace IntelliHome_Backend.Features.PKA.Repositories
{
    public class AirConditionerRepository : CrudRepository<AirConditioner>, IAirConditionerRepository
    {
        public AirConditionerRepository(PostgreSqlDbContext context) : base(context) { }
    }
}

