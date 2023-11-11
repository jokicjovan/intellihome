using Data.Context;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Home.Repositories
{
    public class SmartDeviceRepository : CrudRepository<SmartDevice>, ISmartDeviceRepository
    {
        public SmartDeviceRepository(PostgreSqlDbContext context) : base(context) { }

        public IEnumerable<SmartDevice> FindAllWIthHome()
        {
            return _entities.Include(p => p.SmartHome);
        }

        public IEnumerable<SmartDevice> UpdateAll(List<SmartDevice> smartDevices) 
        {
            _entities.UpdateRange(smartDevices);
            _context.SaveChanges();
            return _entities;
        }
    }
}
