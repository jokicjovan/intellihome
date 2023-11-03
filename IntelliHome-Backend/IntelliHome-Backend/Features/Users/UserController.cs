using Data.Models.Users;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.Users
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<User> register()
        {
            User user = new User();
            user.Username = "crni";
            user.FirstName = "Vule";
            user.LastName = "Bogdan";
            user.Email = "vule@vule";
            user.Password = "12345";
            return await _userService.CreateUser(user);
        }
    }
}
