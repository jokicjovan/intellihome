using Data.Models.Users;
using IntelliHome_Backend.Repositories.Interfaces;
using IntelliHome_Backend.Services.Interfaces;

namespace IntelliHome_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> CreateUser(User user)
        {
            return _userRepository.Create(user);
        }
    }
}
