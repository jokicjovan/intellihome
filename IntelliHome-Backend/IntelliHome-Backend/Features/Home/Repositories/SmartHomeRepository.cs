using Data.Context;
using Data.Models.Home;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;

namespace IntelliHome_Backend.Features.Home.Repositories
{
    public class SmartHomeRepository : CrudRepository<SmartHome>, ISmartHomeRepository
    {
        public SmartHomeRepository(PostgreSqlDbContext context) : base(context) { }
    }
}
