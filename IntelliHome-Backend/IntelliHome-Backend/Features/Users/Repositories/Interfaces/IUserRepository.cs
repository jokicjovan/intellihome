using Data.Models.Users;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.Users.Repositories.Interfaces
{
    public interface IUserRepository : ICrudRepository<User>
    {

    }
}
