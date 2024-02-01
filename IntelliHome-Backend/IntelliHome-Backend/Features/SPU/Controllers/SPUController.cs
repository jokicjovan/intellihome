using Data.Models.Shared;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Infrastructure;
using IntelliHome_Backend.Features.Shared.Services.Interfacted;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntelliHome_Backend.Features.SPU.Controllers
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
        private readonly IDataChangeListener _dataChangeListener;

        public SPUController(ISmartHomeService smartHomeService, ILampService lampService,
            IVehicleGateService vehicleGateService, ISprinklerService sprinklerService,
            IImageService imageService, IDataChangeListener dataChangeListener)
        {
            _smartHomeService = smartHomeService;
            _lampService = lampService;
            _vehicleGateService = vehicleGateService;
            _sprinklerService = sprinklerService;
            _imageService = imageService;
            _dataChangeListener = dataChangeListener;
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateLamp([FromRoute] Guid smartHomeId, [FromForm] LampCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            Lamp lamp = new Lamp
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.SPU,
                Type = SmartDeviceType.LAMP,
                PowerPerHour = dto.PowerPerHour,
                BrightnessLimit = dto.BrightnessLimit,
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            lamp = await _lampService.Create(lamp);
            _dataChangeListener.HandleDataChange(smartHomeId + " " + username);
            return Ok(lamp);

        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateSprinkler([FromRoute] Guid smartHomeId, [FromForm] SprinklerCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            Sprinkler sprinkler = new Sprinkler
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.SPU,
                Type = SmartDeviceType.SPRINKLER,
                PowerPerHour = dto.PowerPerHour,
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            sprinkler = await _sprinklerService.Create(sprinkler);
            _dataChangeListener.HandleDataChange(smartHomeId + " " + username);
            return Ok(sprinkler);

        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateVehicleGate([FromRoute] Guid smartHomeId, [FromForm] VehicleGateCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            VehicleGate vehicleGate = new VehicleGate
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.SPU,
                Type = SmartDeviceType.VEHICLEGATE,
                PowerPerHour = dto.PowerPerHour,
                AllowedLicencePlates = dto.AllowedLicencePlates,
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            vehicleGate = await _vehicleGateService.Create(vehicleGate);
            _dataChangeListener.HandleDataChange(smartHomeId + " " + username);
            return Ok(vehicleGate);

        }
    }
}
