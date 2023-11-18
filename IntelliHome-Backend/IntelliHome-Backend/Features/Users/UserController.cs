﻿using Data.Models.Users;
using IntelliHome_Backend.Features.Users.DTOs;
using IntelliHome_Backend.Features.Users.Services;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.Users
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfirmationService _confirmationService;
        public UserController(IUserService userService, IConfirmationService confirmationService)
        {
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
            String passwordChanged = identity.FindFirst("PasswordChanged").Value;
            return Ok(JsonConvert.SerializeObject(new { role,passwordChanged }, Newtonsoft.Json.Formatting.Indented));
        }

        [HttpPost]
        public async Task<ActionResult<String>> activateAccount(int code)
        {
            await _confirmationService.ActivateAccount(code);
            return Ok("Account activated successfully!");
        }

        [HttpPost]
        [Authorize]
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
        public async Task<ActionResult<AllAdminsDTO>> allAdmins()
        {
            IEnumerable<Admin> admins = await _userService.GetAllAdmins();
            return new AllAdminsDTO(admins);
        }

        [HttpPost]
        public async Task<User> addAdmin([FromForm] UserDTO userDTO)
        {
            Admin admin = new Admin(userDTO.FirstName,userDTO.LastName,userDTO.Email,userDTO.Username,userDTO.Password,true,null,false,false);
            return await _userService.CreateAdmin(admin, userDTO.Image);
        }
    }
    
}
