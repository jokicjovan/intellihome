using Data.Models.Users;

namespace IntelliHome_Backend.Features.Users.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task ChangePassword(Guid id, string password);
        Task<User> CreateAdmin(Admin newAdmin, IFormFile image);
        public Task<User> CreateUser(User user,IFormFile image);
        Task<List<Admin>> GetAllAdmins();
    }
}
