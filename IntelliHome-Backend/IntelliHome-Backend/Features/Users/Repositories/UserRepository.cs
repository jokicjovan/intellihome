using Data.Context;
using Data.Models.Users;
using InfluxDB.Client.Api.Domain;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Users.Repositories
{
    public class UserRepository : CrudRepository<Data.Models.Users.User>, IUserRepository
    {
        public UserRepository(PostgreSqlDbContext context) : base(context) { }

        public async Task<Data.Models.Users.User> FindByEmail(String email)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Email.Equals(email));
        }
        public async Task<Data.Models.Users.User> FindByUsername(String username)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Username.Equals(username));
        }
        public async Task<List<Admin>> GetAllAdmins()
        {
            return await _entities.OfType<Admin>().Where(e=>!e.IsSuperAdmin).ToListAsync();
        }
    }
}
