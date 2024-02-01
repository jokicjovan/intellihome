using Data.Context;
using Data.Models.Home;
using Data.Models.Users;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntelliHome_Backend.Features.Home.Repositories
{
    public class SmartHomeRepository : CrudRepository<SmartHome>, ISmartHomeRepository
    {
        public SmartHomeRepository(PostgreSqlDbContext context) : base(context) { }

        //override read method to include related entities
        public override async Task<SmartHome> Read(Guid Id)
        {
            return await _entities
                .Include(s => s.City)
                .Include(s => s.SmartDevices)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<List<SmartHome>> GetSmartHomesForUser(User user)
        {
            return await _entities                
                .Include(s => s.SmartDevices)
                .Include(s => s.Owner)
                .Include(s => s.City)
                .Where(s => s.Owner.Username == user.Username)
                .ToListAsync();
        }

        public async Task<List<SmartHome>> GetSmartHomesForUserWithNameSearch(User user, string search)
        {
            return await _entities
                .Include(s => s.SmartDevices)
                .Include(s => s.Owner)
                .Include(s => s.City)
                .Where(s => s.Owner.Username == user.Username || s.SmartDevices.Any(t => t.AllowedUsers.Any(u => u.Username == user.Username)))
                .Where(s => s.Name.ToLower().Contains(search.ToLower()))
                .ToListAsync();
        }

        public async Task<List<SmartHome>> GetSmartHomesForApproval()
        {
            return await _entities
                .Include(s => s.SmartDevices)
                .Include(s => s.Owner)
                .Include(s => s.City)
                .Where(s => s.IsApproved == false)
                .ToListAsync();
        }

        public Task<bool> IsUserAllowed(Guid smartHomeId, Guid userId)
        {
            return _entities
                .Where(e => e.Id == smartHomeId && (e.Owner.Id == userId || e.SmartDevices.Any(device => device.AllowedUsers.Any(user => user.Id == userId))))
                .AnyAsync();
        }
    }
}
