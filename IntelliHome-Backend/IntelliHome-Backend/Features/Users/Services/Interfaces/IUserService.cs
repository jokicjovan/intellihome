using Data.Models.Users;

namespace IntelliHome_Backend.Features.Users.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        public Task<User> CreateUser(User user,IFormFile image);
    }
}
