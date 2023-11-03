using Data.Models.Users;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.Users.Services
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
