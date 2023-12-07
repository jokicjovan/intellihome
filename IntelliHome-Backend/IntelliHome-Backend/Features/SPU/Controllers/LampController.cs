using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.SPU.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LampController : ControllerBase
    {
        private readonly ILampService _lampService;

        public LampController(ILampService lampService)
        {
            _lampService = lampService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<LampData> result = _lampService.GetHistoricalData(id, from, to);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            LampDTO result = await _lampService.GetWithData(id);
            return Ok(result);
        }
    }
}
