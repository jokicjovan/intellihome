using Data.Models.PKA;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.PKA
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PKAController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;
        private readonly IAirConditionerService _airConditionerService;
        public PKAController(ISmartHomeService smartHomeService, IAirConditionerService airConditionerService)
        {
            _smartHomeService = smartHomeService;
            _airConditionerService = airConditionerService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAirConditioner([FromQuery] Guid smartHouseId, [FromBody] AirConditionerCreationDTO dto)
        {
            AirConditioner airConditioner = new AirConditioner();
            airConditioner.SmartHome = await _smartHomeService.GetSmartHome(smartHouseId);
            airConditioner.Name = dto.Name;
            airConditioner.Category = Data.Models.Shared.SmartDeviceCategory.PKA;
            airConditioner.CurrentTemperature = dto.CurrentTemperature;
            airConditioner.MinTemperature = dto.MinTemperature;
            airConditioner.MaxTemperature = dto.MaxTemperature;
            airConditioner.PowerPerHour = dto.PowerPerHour;
            airConditioner = await _airConditionerService.CreateAirConditioner(airConditioner);
            return Ok(airConditioner);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAmbientSensor()
        {
            AmbientSensor ambientSensor = new AmbientSensor();
            return Ok(ambientSensor);
        }

        [HttpPost]
        public async Task<ActionResult> CreateWashingMachine()
        {
            WashingMachine washingMachine = new WashingMachine();
            return Ok(washingMachine);
        }
    }
}
