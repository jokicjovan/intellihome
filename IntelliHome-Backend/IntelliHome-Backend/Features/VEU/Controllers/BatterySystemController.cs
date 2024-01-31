using IntelliHome_Backend.Features.VEU.DTOs.BatterySystem;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.VEU.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BatterySystemController : ControllerBase
    {
        private readonly IBatterySystemService _batterySystemService;

        public BatterySystemController(IBatterySystemService batterySystemService)
        {
            _batterySystemService = batterySystemService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCapacityHistoricalData(Guid id, DateTime from, DateTime to)
        {
            if (from > to)
            {
                return BadRequest("FROM date cant be after TO date");
            }
            List<BatterySystemCapacityDataDTO> result = _batterySystemService.GetCapacityHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            BatterySystemDTO result = await _batterySystemService.GetWithCapacityData(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Toggle(Guid id, bool turnOn = true)
        {
            await _batterySystemService.Toggle(id, turnOn);
            return Ok();
        }
    }
}
