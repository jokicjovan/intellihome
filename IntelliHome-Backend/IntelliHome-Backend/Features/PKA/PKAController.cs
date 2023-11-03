using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.PKA
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PKAController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateAirConditioner([FromBody] AirConditionerCreationDTO dto)
        {
            AirConditioner airConditioner = new AirConditioner();
            airConditioner.Name = dto.Name;
            airConditioner.Category = Data.Models.Shared.SmartDeviceCategory.PKA;
            airConditioner.CurrentTemperature = dto.CurrentTemperature;
            airConditioner.MinTemperature = dto.MinTemperature;
            airConditioner.MaxTemperature = dto.MaxTemperature;
            airConditioner.PowerPerHour = dto.PowerPerHour;

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
