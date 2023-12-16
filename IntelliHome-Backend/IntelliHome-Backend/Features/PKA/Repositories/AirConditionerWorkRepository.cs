using Data.Context;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;

namespace IntelliHome_Backend.Features.PKA.Repositories
{
    public class AirConditionerWorkRepository : CrudRepository<AirConditioner>, IAirConditionerWorkRepository
    {
        public AirConditionerWorkRepository(PostgreSqlDbContext context) : base(context) { }
    }
}
