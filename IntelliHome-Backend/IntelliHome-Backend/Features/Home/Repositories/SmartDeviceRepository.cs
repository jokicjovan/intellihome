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
            return _entities.Include(p => p.SmartHome).Include(p=>p.SmartHome.Owner);
        }

        public override async Task<SmartDevice> Read(Guid id)
        {
            return _entities.Include(p => p.SmartHome).Include(p => p.SmartHome.Owner).Include(p=>p.AllowedUsers).FirstOrDefault(p=>p.Id==id);
        }

        public IEnumerable<SmartDevice> FindSmartDevicesForSmartHome(Guid smartHomeId) {
            return _entities.Include(e=> e.SmartHome).ThenInclude(e => e.Owner).Include(e => e.AllowedUsers).Where(e => e.SmartHome.Id == smartHomeId).OrderBy(e => e.Name);
        }

        public IEnumerable<SmartDevice> UpdateAll(List<SmartDevice> smartDevices) 
        {
            _entities.UpdateRange(smartDevices);
            _context.SaveChanges();
            return _entities;
        }

        public Task<bool> IsUserAllowed(Guid smartDeviceId, Guid userId) {
            return _entities.AnyAsync(e => e.Id == smartDeviceId && (e.AllowedUsers.Any(user => user.Id == userId) || e.SmartHome.Owner.Id == userId));
        }

        public Task<SmartDevice> FindWithSmartHome(Guid id)
        {
            return _entities.Include(e => e.SmartHome).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
