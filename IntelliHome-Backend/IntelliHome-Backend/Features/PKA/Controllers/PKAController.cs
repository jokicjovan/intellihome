using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfacted;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.PKA.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PKAController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;
        private readonly IAirConditionerService _airConditionerService;
        private readonly IAmbientSensorService _ambientSensorService;
        private readonly IWashingMachineService _washingMachineService;
        private readonly IImageService _imageService;

        public PKAController(ISmartHomeService smartHomeService, IAirConditionerService airConditionerService,
            IAmbientSensorService ambientSensorService, IWashingMachineService washingMachineService,
            IImageService imageService)
        {
            _smartHomeService = smartHomeService;
            _airConditionerService = airConditionerService;
            _ambientSensorService = ambientSensorService;
            _washingMachineService = washingMachineService;
            _imageService = imageService;
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateAirConditioner([FromRoute] Guid smartHomeId, [FromForm] AirConditionerCreationDTO dto)
        {
            AirConditioner airConditioner = new AirConditioner
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.PKA,
                Type = SmartDeviceType.AIRCONDITIONER,
                PowerPerHour = dto.PowerPerHour,
                MinTemperature = dto.MinTemperature,
                MaxTemperature = dto.MaxTemperature,
                Modes = dto.Modes,
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            airConditioner = await _airConditionerService.Create(airConditioner);
            return Ok(airConditioner);
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateAmbientSensor([FromRoute] Guid smartHomeId, [FromForm] AmbientSensorCreationDTO dto)
        {
            AmbientSensor ambientSensor = new AmbientSensor
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.PKA,
                Type = SmartDeviceType.AMBIENTSENSOR,
                PowerPerHour = dto.PowerPerHour,
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            ambientSensor = await _ambientSensorService.Create(ambientSensor);
            return Ok(ambientSensor);
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateWashingMachine([FromRoute] Guid smartHomeId, [FromForm] WashingMachineCreationDTO dto)
        {
            WashingMachine washingMachine = new WashingMachine
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.PKA,
                Type = SmartDeviceType.WASHINGMACHINE,
                PowerPerHour = dto.PowerPerHour,
                Modes = _washingMachineService.GetWashingMachineModes(dto.ModesIds),
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            washingMachine = await _washingMachineService.Create(washingMachine);
            return Ok(washingMachine);
        }

        [HttpGet]
        public async Task<ActionResult> GetWashingMachineModes()
        {
            IEnumerable<WashingMachineMode> allWashingMachineModes = await _washingMachineService.GetAllWashingMachineModes();
            return Ok(allWashingMachineModes);
        }
    }
}
