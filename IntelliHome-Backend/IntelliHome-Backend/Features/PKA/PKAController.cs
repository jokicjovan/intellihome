using Data.Models.PKA;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services;
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
        public async Task<ActionResult> CreateAirConditioner([FromQuery] Guid smartHomeId, [FromForm] AirConditionerCreationDTO dto)
        {
            AirConditioner airConditioner = new AirConditioner
            {
                SmartHome = await _smartHomeService.GetSmartHome(smartHomeId),
                Name = dto.Name,
                Category = Data.Models.Shared.SmartDeviceCategory.PKA,
                PowerPerHour = dto.PowerPerHour,
                MinTemperature = dto.MinTemperature,
                MaxTemperature = dto.MaxTemperature,
                Modes = dto.Modes,
                Image = (dto.Image != null && dto.Image.Length > 0) ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            airConditioner = await _airConditionerService.CreateAirConditioner(airConditioner);
            return Ok(airConditioner);

        }

        [HttpPost]
        public async Task<ActionResult> CreateAmbientSensor([FromQuery] Guid smartHomeId, [FromForm] AmbientSensorCreationDTO dto)
        {
            AmbientSensor ambientSensor = new AmbientSensor
            {
                SmartHome = await _smartHomeService.GetSmartHome(smartHomeId),
                Name = dto.Name,
                Category = Data.Models.Shared.SmartDeviceCategory.PKA,
                PowerPerHour = dto.PowerPerHour,
                Image = (dto.Image != null && dto.Image.Length > 0) ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            ambientSensor = await _ambientSensorService.CreateAmbientSensor(ambientSensor);
            return Ok(ambientSensor);

        }

        [HttpPost]
        public async Task<ActionResult> CreateWashingMachine([FromQuery] Guid smartHomeId, [FromForm] WashingMachineCreationDTO dto)
        {
            WashingMachine washingMachine = new WashingMachine
            {
                SmartHome = await _smartHomeService.GetSmartHome(smartHomeId),
                Name = dto.Name,
                Category = Data.Models.Shared.SmartDeviceCategory.PKA,
                PowerPerHour = dto.PowerPerHour,
                Modes = _washingMachineService.GetWashingMachineModes(dto.ModesIds),
                Image = (dto.Image != null && dto.Image.Length > 0) ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            washingMachine = await _washingMachineService.CreateWashingMachine(washingMachine);
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
