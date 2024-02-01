using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IntelliHome_Backend.Features.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using IntelliHome_Backend.Features.SPU.DTOs;
using Data.Models.Users;
using IntelliHome_Backend.Features.Users.Services.Interfaces;

namespace IntelliHome_Backend.Features.Home.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmartHomeController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;
        private readonly IUserService _userService;

        public SmartHomeController(ISmartHomeService smartHomeService,IUserService userService)
        {
            _smartHomeService = smartHomeService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateSmartHome([FromForm] SmartHomeCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            GetSmartHomeDTO smartHome;
            try
            {
                smartHome = await _smartHomeService.CreateSmartHome(dto, username);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(smartHome);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetSmartHome(Guid Id)
        {
            GetSmartHomeDTO smartHome;
            try
            {
                smartHome = await _smartHomeService.GetSmartHomeDTOO(Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(smartHome);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetSmartHomesForUser([FromQuery] PageParametersDTO pageParameters, [FromQuery] string search)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            SmartHomePaginatedDTO smartHomes;
            try
            {
                if (search == null)
                {
                    search = "";
                }
                // remove first and last character from search string
                search = search.Substring(1, search.Length - 2);

                smartHomes = await _smartHomeService.GetSmartHomesForUser(username, search, pageParameters);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(smartHomes);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllSmartHomesPaged([FromQuery] PageParametersDTO pageParameters, [FromQuery] string search)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            SmartHomePaginatedDTO smartHomes;
            try
            {
                if (search == null)
                {
                    search = "";
                }
                search = search.Substring(1, search.Length - 2);
                smartHomes = await _smartHomeService.GetAllPaged(search, pageParameters);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(smartHomes);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetSmartHomesForApproval([FromQuery] PageParametersDTO pageParameters)
        {
            SmartHomePaginatedDTO smartHomes;
            try
            {
                smartHomes = await _smartHomeService.GetSmartHomesForApproval(pageParameters);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(smartHomes);
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ApproveSmartHome(Guid id)
        {
            try
            {
                await _smartHomeService.ApproveSmartHome(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Smart home approved!");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> IsOwner(String homeId)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool isOwner = Guid.Parse(id)==(await _smartHomeService.Get(Guid.Parse(homeId))).Owner.Id;
            return Ok(isOwner);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllEmailsWithPermission(string homeId)
        {
            List<String> emails = new List<string>();
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Data.Models.Users.User user = await _userService.Get(Guid.Parse(id));
            SmartHome smarthome = await _smartHomeService.Get(Guid.Parse(homeId));
            if (user.Id.Equals(smarthome.Owner.Id))
            {
                emails=await _smartHomeService.GetAllEmailsWithPermission(smarthome);
            }
            else
            {
                return BadRequest("Permission Denied");
            }

            return Ok(emails);
        }


        [HttpPut]
        [Authorize]
        public async Task<ActionResult> AddPermission([FromBody] PermissionDTO permission)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Data.Models.Users.User user = await _userService.Get(Guid.Parse(id));
            SmartHome smarthome = await _smartHomeService.Get(Guid.Parse(permission.home));
            if (user.Id.Equals(smarthome.Owner.Id))
            {
                await _smartHomeService.AddPermision(smarthome, permission.user);
            }
            else
            {
                return BadRequest("Permission Denied");
            }

            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> RemovePermission([FromBody] PermissionDTO permission)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            String passwordChanged = null;
            Data.Models.Users.User user = await _userService.Get(Guid.Parse(id));
            SmartHome smarthome = await _smartHomeService.Get(Guid.Parse(permission.home));
            if (user.Id.Equals(smarthome.Owner.Id))
            {
                await _smartHomeService.RemovePermision(smarthome, permission.user);
            }
            else
            {
                return BadRequest("Permission Denied");
            }

            return Ok();
        }



        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSmartHome(Guid id, Guid userId, string reason)
        {
            try
            {
                await _smartHomeService.DeleteSmartHome(id, userId, reason);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Smart home deleted!");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsageHistoricalData(Guid id, DateTime from, DateTime to)
        {
            if (from > to) {
                return BadRequest("FROM date cant be after TO date");
            }
            List<SmartHomeUsageDataDTO> result = _smartHomeService.GetUsageHistoricalData(id, from, to);
            return Ok(result);
        }
    }
}