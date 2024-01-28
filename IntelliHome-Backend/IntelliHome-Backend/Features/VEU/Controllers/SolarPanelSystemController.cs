using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntelliHome_Backend.Features.VEU.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SolarPanelSystemController : ControllerBase
    {
        private readonly ISolarPanelSystemService _solarPanelSystemService;

        public SolarPanelSystemController(ISolarPanelSystemService solarPanelSystemService)
        {
            _solarPanelSystemService = solarPanelSystemService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProductionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<SolarPanelSystemProductionDataDTO> result = _solarPanelSystemService.GetProductionHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            SolarPanelSystemDTO result = await _solarPanelSystemService.GetWithProductionData(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Toggle(Guid id, bool turnOn = true)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded) 
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            await _solarPanelSystemService.Toggle(id, username, turnOn);
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
            List<ActionDataDTO> result = _solarPanelSystemService.GetActionHistoricalData(id, from, to);
            return Ok(result);
        }
    }
}
