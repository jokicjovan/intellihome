using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IntelliHome_Backend.Features.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace IntelliHome_Backend.Features.Home.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmartHomeController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;

        public SmartHomeController(ISmartHomeService smartHomeService)
        {
            _smartHomeService = smartHomeService;
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
                smartHome = await _smartHomeService.GetSmartHomeDTO(Id);
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