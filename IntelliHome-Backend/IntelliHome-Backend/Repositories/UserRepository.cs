using Data.Context;
using Data.Models.Users;
using IntelliHome_Backend.Repositories.Interfaces;

namespace IntelliHome_Backend.Repositories
{
    public class UserRepository : CrudRepository<User>, IUserRepository
    {
        public UserRepository(PostgreSqlDbContext context) : base(context) { }


    }
}
