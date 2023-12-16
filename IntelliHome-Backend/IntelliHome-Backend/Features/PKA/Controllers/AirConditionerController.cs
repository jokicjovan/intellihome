using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            await _airConditionerService.AddScheduledWork(schedule.Id,schedule.Temperature,schedule.Mode,schedule.StartDate,schedule.EndDate);
            return Ok();
        }


        [HttpPut]
        public async Task<ActionResult> Toggle(Guid id, bool turnOn = true)
        {
            await _airConditionerService.ToggleAmbientSensor(id, turnOn);
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
            await _airConditionerService.ChangeTemperature(id, temperature);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeMode(Guid id, string mode)
        {
            await _airConditionerService.ChangeMode(id, mode);
            return Ok();
        }
    }
}
