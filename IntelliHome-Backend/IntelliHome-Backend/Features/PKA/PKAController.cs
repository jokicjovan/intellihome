using Data.Models.PKA;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.PKA
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PKAController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;
        private readonly IAirConditionerService _airConditionerService;
        private readonly IAmbientSensorService _ambientSensorService;
        private readonly IWashingMachineService _washingMachineService;
        private readonly IWashingMachineModeService _washingMachineModeService;

        public PKAController(ISmartHomeService smartHomeService, IAirConditionerService airConditionerService,
            IAmbientSensorService ambientSensorService, IWashingMachineService washingMachineService, IWashingMachineModeService washingMachineModeService)
        {
            _smartHomeService = smartHomeService;
            _airConditionerService = airConditionerService;
            _ambientSensorService = ambientSensorService;
            _washingMachineService = washingMachineService;
            _washingMachineModeService = washingMachineModeService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAirConditioner([FromQuery] Guid smartHomeId, [FromBody] AirConditionerCreationDTO dto)
        {
            AirConditioner airConditioner = new AirConditioner();
            airConditioner.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            airConditioner.Name = dto.Name;
            airConditioner.Category = Data.Models.Shared.SmartDeviceCategory.PKA;
            airConditioner.PowerPerHour = dto.PowerPerHour;
            airConditioner.MinTemperature = dto.MinTemperature;
            airConditioner.MaxTemperature = dto.MaxTemperature;
            airConditioner.Modes = dto.Modes;
            airConditioner.CurrentMode = airConditioner.Modes[0];
            airConditioner = await _airConditionerService.CreateAirConditioner(airConditioner);
            return Ok(airConditioner);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAmbientSensor([FromQuery] Guid smartHomeId, [FromBody] SmartDeviceDTO dto)
        {
            AmbientSensor ambientSensor = new AmbientSensor();
            ambientSensor.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            ambientSensor.Name = dto.Name;
            ambientSensor.Category = Data.Models.Shared.SmartDeviceCategory.PKA;
            ambientSensor.PowerPerHour = dto.PowerPerHour;
            ambientSensor = await _ambientSensorService.CreateAmbientSensor(ambientSensor);
            return Ok(ambientSensor);
        }

        [HttpPost]
        public async Task<ActionResult> CreateWashingMachine([FromQuery] Guid smartHomeId, [FromBody] WashingMachineCreationDTO dto)
        {
            WashingMachine washingMachine = new WashingMachine();
            washingMachine.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            washingMachine.Name = dto.Name;
            washingMachine.Category = Data.Models.Shared.SmartDeviceCategory.PKA;
            washingMachine.PowerPerHour = dto.PowerPerHour;
            washingMachine.Modes = _washingMachineModeService.GetWashingMachineModes(dto.ModesIds);
            washingMachine = await _washingMachineService.CreateWashingMachine(washingMachine);
            return Ok(washingMachine);
        }
    }
}
