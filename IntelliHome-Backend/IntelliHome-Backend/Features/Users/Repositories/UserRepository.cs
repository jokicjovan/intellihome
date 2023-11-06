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

        public async Task<User> Read(String username)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
