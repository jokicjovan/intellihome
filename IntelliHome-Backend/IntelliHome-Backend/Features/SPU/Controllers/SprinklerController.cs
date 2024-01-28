using System.Security.Claims;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.SPU.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SprinklerController : ControllerBase
    {
        private readonly ISprinklerService _sprinklerService;

        public SprinklerController(ISprinklerService sprinklerService)
        {
            _sprinklerService = sprinklerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<SprinklerData> result = _sprinklerService.GetHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddScheduledWork(SprinklerSchedulerCreationDTO schedule)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            await _sprinklerService.AddScheduledWork(schedule.Id, schedule.IsSpraying, schedule.StartDate, schedule.EndDate, username);
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
            await _sprinklerService.ToggleSprinkler(id, username, turnOn);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            SprinklerDTO result = await _sprinklerService.GetWithData(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> ToggleSprinklerSpraying(Guid id, bool turnOn = true)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            await _sprinklerService.ToggleSprinklerSpraying(id, username, turnOn);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<ActionDataDTO> result = _sprinklerService.GetActionHistoricalData(id, from, to);
            return Ok(result);
        }


    }
}
