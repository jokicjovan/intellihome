using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetProductionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<SolarPanelSystemProductionDataDTO> result = _solarPanelSystemService.GetProductionHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            SolarPanelSystemDTO result = await _solarPanelSystemService.GetWithProductionData(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Toggle(Guid id, bool turnOn = true)
        {
            await _solarPanelSystemService.ToggleSolarPanelSystem(id, turnOn);
            return Ok();
        }
    }
}
