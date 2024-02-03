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
    public class WashingMachineController:ControllerBase
    {
        private readonly IWashingMachineService _washingMachineService;

        public WashingMachineController(IWashingMachineService washingMachineService)
        {
            _washingMachineService = washingMachineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<WashingMachineData> result = _washingMachineService.GetHistoricalData(id, from, to);
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
            await _washingMachineService.AddScheduledWork(schedule.Id, schedule.Temperature, schedule.Mode, schedule.StartDate, schedule.EndDate, username);
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
            await _washingMachineService.ToggleWashingMachine(id, username, turnOn);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            WashingMachineDTO result = await _washingMachineService.GetWithData(id);
            return Ok(result);
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
            await _washingMachineService.ChangeMode(id, mode, username);
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
            List<ActionDataDTO> result = _washingMachineService.GetActionHistoricalData(id, from, to);
            return Ok(result);
        }
    }
}
