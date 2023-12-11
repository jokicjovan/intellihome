using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.VEU.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BatteryController : ControllerBase
    {
        private readonly IBatterySystemService _batterySystemService;

        public BatteryController(IBatterySystemService batterySystemService)
        {
            _batterySystemService = batterySystemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<BatterySystemDataDTO> result = _batterySystemService.GetHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            BatterySystemDTO result = await _batterySystemService.GetWithData(id);
            return Ok(result);
        }
    }
}
