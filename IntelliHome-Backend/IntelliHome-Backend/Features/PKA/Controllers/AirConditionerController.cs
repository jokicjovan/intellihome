using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntelliHome_Backend.Features.PKA.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AirConditionerController:ControllerBase
    {
        private readonly IAirConditionerService _airConditionerService;

        public AirConditionerController(IAirConditionerService airConditionerService)
        {
            _airConditionerService = airConditionerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<AirConditionerData> result = _airConditionerService.GetHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddScheduledWork(ACSchedulerCreationDTO schedule)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            await _airConditionerService.AddScheduledWork(schedule.Id,schedule.Temperature,schedule.Mode,schedule.StartDate,schedule.EndDate, username);
            return Ok();
        }


        [HttpPut]
        public async Task<ActionResult> Toggle(Guid id, bool turnOn = true)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            await _airConditionerService.ToggleAirConditioner(id,username, turnOn);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            AirConditionerDTO result = await _airConditionerService.GetWithData(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeTemperature(Guid id, Double temperature)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            await _airConditionerService.ChangeTemperature(id, temperature,username);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeMode(Guid id, string mode)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            await _airConditionerService.ChangeMode(id, mode, username);
            return Ok();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            if (from > to)
            {
                return BadRequest("FROM date cant be after TO date");
            }
            List<ActionDataDTO> result = _airConditionerService.GetActionHistoricalData(id, from, to);
            return Ok(result);
        }
    }
}
