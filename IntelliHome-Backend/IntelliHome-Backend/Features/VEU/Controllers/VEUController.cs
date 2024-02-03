using Data.Models.Shared;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Infrastructure;
using IntelliHome_Backend.Features.Shared.Services.Interfacted;
using IntelliHome_Backend.Features.VEU.DTOs.BatterySystem;
using IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntelliHome_Backend.Features.VEU.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VEUController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;
        private readonly IBatterySystemService _batterySystemService;
        private readonly ISolarPanelSystemService _solarPanelSystemService;
        private readonly IVehicleChargerService _vehicleChargerService;
        private readonly IImageService _imageService;
        private readonly IDataChangeListener _dataChangeListener;

        public VEUController(ISmartHomeService smartHomeService, IBatterySystemService batterySystemService,
                ISolarPanelSystemService solarPanelSystemService, IVehicleChargerService vehicleChargerService,
                IImageService imageService, IDataChangeListener dataChangeListener)
        {
            _smartHomeService = smartHomeService;
            _batterySystemService = batterySystemService;
            _solarPanelSystemService = solarPanelSystemService;
            _vehicleChargerService = vehicleChargerService;
            _imageService = imageService;
            _dataChangeListener = dataChangeListener;
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        [Authorize]
        public async Task<ActionResult> CreateBatterySystem([FromRoute] Guid smartHomeId, [FromForm] BatterySystemCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            BatterySystem batterySystem = new BatterySystem
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.VEU,
                Type = SmartDeviceType.BATTERYSYSTEM,
                Capacity = dto.Capacity,
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            batterySystem = await _batterySystemService.Create(batterySystem);
            _dataChangeListener.HandleDataChange(smartHomeId + " " + username);
            return Ok(batterySystem);
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        [Authorize]
        public async Task<ActionResult> CreateSolarPanelSystem([FromRoute] Guid smartHomeId, [FromForm] SolarPanelSystemCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            SolarPanelSystem solarPanelSystem = new SolarPanelSystem
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.VEU,
                Type = SmartDeviceType.SOLARPANELSYSTEM,
                Area = dto.Area,
                Efficiency = dto.Efficiency,
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            solarPanelSystem = await _solarPanelSystemService.Create(solarPanelSystem);
            _dataChangeListener.HandleDataChange(smartHomeId + " " + username);
            return Ok(solarPanelSystem);
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        [Authorize]
        public async Task<ActionResult> CreateVehicleCharger([FromRoute] Guid smartHomeId, [FromForm] VehicleChargerCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
            VehicleCharger vehicleCharger = new VehicleCharger
            {
                SmartHome = await _smartHomeService.Get(smartHomeId),
                Name = dto.Name,
                Category = SmartDeviceCategory.VEU,
                Type = SmartDeviceType.VEHICLECHARGER,
                PowerPerHour = dto.PowerPerHour,
                ChargingPoints = Enumerable.Range(0, dto.NumberOfChargingPoints).Select(_ => new VehicleChargingPoint { IsFree = true }).ToList(),
                Image = dto.Image != null && dto.Image.Length > 0 ? _imageService.SaveDeviceImage(dto.Image) : null
            };
            vehicleCharger = await _vehicleChargerService.Create(vehicleCharger);
            _dataChangeListener.HandleDataChange(smartHomeId + " " + username);
            return Ok(vehicleCharger);
        }
    }
}
