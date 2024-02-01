using Data.Models.Users;
using IntelliHome_Backend.Features.Users.DTOs;
using IntelliHome_Backend.Features.Users.Services;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using InfluxDB.Client.Api.Domain;
using User = Data.Models.Users.User;
using IntelliHome_Backend.Features.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;

namespace IntelliHome_Backend.Features.Users
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfirmationService _confirmationService;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration, IUserService userService, IConfirmationService confirmationService)
        {
            _configuration = configuration;
            _userService = userService;
            _confirmationService = confirmationService;
        }

        [HttpPost]
        public async Task<User> register([FromForm] UserDTO userDTO)
        {
            User user = new User(userDTO.FirstName,userDTO.LastName,userDTO.Email,userDTO.Username,userDTO.Password,false,null);
            return await _userService.CreateUser(user,userDTO.Image);
        }

        [HttpPost]
        public async Task<ActionResult<String>> login(CredentialsDTO credentialsDTO)
        {
            User user = await _userService.Authenticate(credentialsDTO.Username, credentialsDTO.Password);
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Role, user.GetType().Name));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            if (user.GetType()  == typeof(Admin)) {
                Admin admin= (Admin)user;
                identity.AddClaim(new Claim("PasswordChanged", admin.PasswordChanged.ToString()));
            }
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return Ok("Logged in successfully!");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> whoAmI()
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            String role = identity.FindFirst(ClaimTypes.Role).Value;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            String passwordChanged = null;
            Data.Models.Users.User user =await _userService.Get(Guid.Parse(id));
            if (user.GetType() == typeof(Admin))
            {
                Admin admin = (Admin)user;
                passwordChanged = admin.PasswordChanged.ToString();
            }

            return Ok(JsonConvert.SerializeObject(new { role,passwordChanged,id }, Newtonsoft.Json.Formatting.Indented));
        }

        [HttpPost]
        public async Task<ActionResult<String>> activateAccount(int code)
        {
            await _confirmationService.ActivateAccount(code);
            return Ok("Account activated successfully!");
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<String>> changePassword(ChangePasswordDTO changePasswordDTO)
        {
            await _userService.ChangePassword(changePasswordDTO.Id,changePasswordDTO.Password);
            return Ok("Password changed successfully!");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<String>> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out successfully!");
        }
        [HttpGet] 
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AllAdminsDTO>> allAdmins()
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = await _userService.Get(Guid.Parse(id));
            if (user.GetType() == typeof(Admin))
            {
                Admin me = (Admin)user;
                if (!me.IsSuperAdmin) return Forbid("Authentication error!");
            }
            else
            {
                Forbid("Authentication error!");
            }
            IEnumerable<Admin> admins = await _userService.GetAllAdmins();
            return new AllAdminsDTO(admins);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> addAdmin([FromForm] UserDTO userDTO)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = await _userService.Get(Guid.Parse(id));
            if (user.GetType() == typeof(Admin))
            {
                Admin me = (Admin)user;
                if (!me.IsSuperAdmin) return Forbid("Authentication error!");
            }
            else
            {
                Forbid("Authentication error!");
            }
            Admin admin = new Admin(userDTO.FirstName,userDTO.LastName,userDTO.Email,userDTO.Username,userDTO.Password,true,null,false,false);
            return await _userService.CreateAdmin(admin, userDTO.Image);
        }
        [HttpGet("/api/User/signin-google")]
        public IActionResult Login2()
        {
            var redirectUrl = Url.Action("GoogleCallback");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet]
        [Route("/api/User/auth/google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                throw new InvalidInputException("Authentication failed.");
            }
            var username = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            User user = await _userService.GetByEmail(email);
            if (user == null)
            {

                User userDTO = new User(name.Substring(0, name.IndexOf(' ')), name.Substring(name.IndexOf(' ')), email,username,GenerateRandomPassword(),true,null);
                User newUser = await _userService.CreateUser(userDTO, null);
                user = newUser;
            }
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                String role = "User";
                if (user.GetType() == typeof(Admin)) role = "Admin";
                identity.AddClaim(new Claim(ClaimTypes.Role,role));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                if (user.GetType() == typeof(Admin))
                {
                    Admin admin = (Admin)user;
                    identity.AddClaim(new Claim("PasswordChanged", admin.PasswordChanged.ToString()));
                }
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Redirect(_configuration["Client:Host"] + "/home");
        }
        private string GenerateRandomPassword()
        {
            PasswordOptions opts = new PasswordOptions()
            {
                RequiredLength = 12,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
    
}
