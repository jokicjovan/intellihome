using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.PKA.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AmbientSensorController : ControllerBase
    {
        private readonly IAmbientSensorService _ambientSensorService;

        public AmbientSensorController(IAmbientSensorService ambientSensorService)
        {
            _ambientSensorService = ambientSensorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<AmbientSensorData> result = _ambientSensorService.GetHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            AmbientSensorDTO result = await _ambientSensorService.GetWithData(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLastHourData(Guid id)
        {
            List<AmbientSensorData> result = _ambientSensorService.GetLastHourData(id);
            return Ok(result);
        }
    }
}
