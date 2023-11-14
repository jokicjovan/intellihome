using Data.Models.Shared;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Services;
using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.VEU
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

        public VEUController(ISmartHomeService smartHomeService, IBatterySystemService batterySystemService, 
                ISolarPanelSystemService solarPanelSystemService, IVehicleChargerService vehicleChargerService,
                IImageService imageService)
        {
            _smartHomeService = smartHomeService;
            _batterySystemService = batterySystemService;
            _solarPanelSystemService = solarPanelSystemService;
            _vehicleChargerService = vehicleChargerService;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateBatterySystem([FromQuery] Guid smartHomeId, [FromForm] BatterySystemCreationDTO dto)
        {
            BatterySystem batterySystem = new BatterySystem();
            batterySystem.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            batterySystem.Name = dto.Name;
            batterySystem.Category = SmartDeviceCategory.VEU;
            batterySystem.Capacity = dto.Capacity;
            if (dto.Image != null && dto.Image.Length > 0) batterySystem.Image = _imageService.SaveDeviceImage(dto.Image);
            batterySystem = await _batterySystemService.CreateBatterySystem(batterySystem);
            return Ok(batterySystem);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSolarPanelSystem([FromQuery] Guid smartHomeId, [FromForm] SolarPanelSystemCreationDTO dto)
        {
            SolarPanelSystem solarPanelSystem = new SolarPanelSystem();
            solarPanelSystem.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            solarPanelSystem.Name = dto.Name;
            solarPanelSystem.Category = SmartDeviceCategory.VEU;
            solarPanelSystem.Area = dto.Area;
            solarPanelSystem.Efficiency = dto.Efficiency;
            if (dto.Image != null && dto.Image.Length > 0) solarPanelSystem.Image = _imageService.SaveDeviceImage(dto.Image);
            solarPanelSystem = await _solarPanelSystemService.CreateSolarPanelSystem(solarPanelSystem);
            return Ok(solarPanelSystem);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicleCharger([FromQuery] Guid smartHomeId, [FromForm] VehicleChargerCreationDTO dto)
        {
            VehicleCharger vehicleCharger = new VehicleCharger();
            vehicleCharger.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            vehicleCharger.Name = dto.Name;
            vehicleCharger.Category = SmartDeviceCategory.VEU;
            vehicleCharger.Power = dto.Power;
            if (dto.Image != null && dto.Image.Length > 0) vehicleCharger.Image = _imageService.SaveDeviceImage(dto.Image);
            vehicleCharger = await _vehicleChargerService.CreateVehicleCharger(vehicleCharger);
            return Ok(vehicleCharger);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicleChargingPoint([FromQuery] Guid vehicleChargerId)
        {
            VehicleChargingPoint vehicleChargingPoint = new VehicleChargingPoint();
            vehicleChargingPoint.IsFree = true;
            vehicleChargingPoint.VehicleCharger = await _vehicleChargerService.GetVehicleCharger(vehicleChargerId);
            vehicleChargingPoint = await _vehicleChargerService.CreateVehicleChargingPoint(vehicleChargingPoint);
            return Ok(vehicleChargingPoint);
        }
    }
}
