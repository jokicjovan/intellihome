using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
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
        public async Task<IActionResult> GetCapacityHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<BatterySystemCapacityDataDTO> result = _batterySystemService.GetCapacityHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            BatterySystemDTO result = await _batterySystemService.GetWithCapacityData(id);
            return Ok(result);
        }
    }
}
