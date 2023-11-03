using IntelliHome_Backend.Repositories.Interfaces;
using Data.Models.Users;
using IntelliHome_Backend.Features.Users.Interfaces;

namespace IntelliHome_Backend.Features.Users
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
