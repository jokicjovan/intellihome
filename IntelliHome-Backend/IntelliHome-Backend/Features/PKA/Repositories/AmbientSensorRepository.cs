using Data.Context;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.PKA.Repositories
{
    public class AmbientSensorRepository : CrudRepository<AmbientSensor>, IAmbientSensorRepository
    {
        public AmbientSensorRepository(PostgreSqlDbContext context) : base(context) { }

        public IEnumerable<AmbientSensor> FindAllWIthHome()
        {
            return _entities.Include(p => p.SmartHome);
        }
}
}
