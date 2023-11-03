using Data.Models.Users;

namespace IntelliHome_Backend.Features.Users.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> CreateUser(User user);
    }
}
