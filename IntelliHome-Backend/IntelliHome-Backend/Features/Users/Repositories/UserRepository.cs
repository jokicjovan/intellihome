using Data.Context;
using Data.Models.Users;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Users.Repositories
{
    public class UserRepository : CrudRepository<User>, IUserRepository
    {
        public UserRepository(PostgreSqlDbContext context) : base(context) { }

        public async Task<User> FindByEmail(String email)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Email.Equals(email));
        }
        public async Task<User> FindByUsername(String username)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Username.Equals(username));
        }
    }
}
