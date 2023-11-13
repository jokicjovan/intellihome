using Data.Context;
using Data.Models.Home;
using Data.Models.Users;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

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
                .Where(s => s.Owner.Username == user.Username)
                .Where(s => s.Name.ToLower().Contains(search.ToLower()))
                .ToListAsync();
        }
    }
}
