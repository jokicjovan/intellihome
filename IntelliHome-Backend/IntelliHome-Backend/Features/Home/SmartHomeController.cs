using System.ComponentModel.DataAnnotations;
using Data.Models.Home;
using Data.Models.PKA;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.Home
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
        public async Task<ActionResult> CreateSmartHome([FromForm] SmartHomeCreationDTO dto)
        {
            // TODO: Get user from token
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            String username = identity.FindFirst(ClaimTypes.Name).Value;
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
        public async Task<ActionResult> GetSmartHomesForUser([FromQuery] PageParametersDTO pageParameters, [FromQuery] String search)
        {
            // TODO: Get user from token
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            String username = identity.FindFirst(ClaimTypes.Name).Value;
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


        //TODO: Add authorization, just admin can approve smart homes
        [HttpPut]
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


        // TODO: Add authorization, just admin can delete smart homes or owner of the smart home
        [HttpDelete]
        public async Task<ActionResult> DeleteSmartHome(Guid id)
        {
            try
            {
                await _smartHomeService.DeleteSmartHome(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Smart home deleted!");
        }
    }
}