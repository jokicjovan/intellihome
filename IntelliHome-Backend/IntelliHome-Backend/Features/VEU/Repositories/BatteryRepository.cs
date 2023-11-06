using Data.Context;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Repositories
{
    public class BatteryRepository : CrudRepository<Battery>, IBatteryRepository
    {
        public BatteryRepository(PostgreSqlDbContext context) : base(context) { }
    }
}
