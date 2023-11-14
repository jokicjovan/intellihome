using Data.Models.PKA;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.SPU
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SPUController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;
        private readonly ILampService _lampService;
        private readonly IVehicleGateService _vehicleGateService;
        private readonly ISprinklerService _sprinklerService;
        private readonly IImageService _imageService;

        public SPUController(ISmartHomeService smartHomeService, ILampService lampService,
            IVehicleGateService vehicleGateService, ISprinklerService sprinklerService,
            IImageService imageService)
        {
            _smartHomeService = smartHomeService;
            _lampService = lampService;
            _vehicleGateService = vehicleGateService;
            _sprinklerService = sprinklerService;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateLamp([FromQuery] Guid smartHomeId, [FromForm] LampCreationDTO dto)
        {
            Lamp lamp = new Lamp
            {
                SmartHome = await _smartHomeService.GetSmartHome(smartHomeId),
                Name = dto.Name,
                Category = Data.Models.Shared.SmartDeviceCategory.SPU,
                PowerPerHour = dto.PowerPerHour,
                BrightnessLimit = dto.BrightnessLimit,
                Image = (dto.Image != null && dto.Image.Length > 0) ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            lamp = await _lampService.CreateLamp(lamp);
            return Ok(lamp);

        }

        [HttpPost]
        public async Task<ActionResult> CreateSprinkler([FromQuery] Guid smartHomeId, [FromForm] SprinklerCreationDTO dto)
        {
            Sprinkler sprinkler = new Sprinkler
            {
                SmartHome = await _smartHomeService.GetSmartHome(smartHomeId),
                Name = dto.Name,
                Category = Data.Models.Shared.SmartDeviceCategory.SPU,
                PowerPerHour = dto.PowerPerHour,
                Image = (dto.Image != null && dto.Image.Length > 0) ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            sprinkler = await _sprinklerService.CreateSprinkler(sprinkler);
            return Ok(sprinkler);

        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicleGate([FromQuery] Guid smartHomeId, [FromForm] VehicleGateCreationDTO dto)
        {
            VehicleGate vehicleGate = new VehicleGate
            {
                SmartHome = await _smartHomeService.GetSmartHome(smartHomeId),
                Name = dto.Name,
                Category = Data.Models.Shared.SmartDeviceCategory.SPU,
                PowerPerHour = dto.PowerPerHour,
                AllowedLicencePlates = dto.AllowedLicencePlates,
                Image = (dto.Image != null && dto.Image.Length > 0) ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            vehicleGate = await _vehicleGateService.CreateVehicleGate(vehicleGate);
            return Ok(vehicleGate);

        }
    }
}
