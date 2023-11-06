using Data.Models.Shared;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
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
        private readonly IBatteryService _batteryService;
        private readonly ISolarPanelService _solarPanelService;
        private readonly IVehicleChargerService _vehicleChargerService;

        public VEUController(ISmartHomeService smartHomeService, IBatteryService batteryService, ISolarPanelService solarPanelService, IVehicleChargerService vehicleChargerService)
        {
            _smartHomeService = smartHomeService;
            _batteryService = batteryService;
            _solarPanelService = solarPanelService;
            _vehicleChargerService = vehicleChargerService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateBatterySystem([FromQuery] Guid smartHomeId, [FromBody] SmartDeviceDTO dto)
        {
            BatterySystem batterySystem = new BatterySystem();
            batterySystem.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            batterySystem.Name = dto.Name;
            batterySystem.Category = SmartDeviceCategory.VEU;
            batterySystem = await _batteryService.CreateBatterySystem(batterySystem);
            return Ok(batterySystem);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBattery([FromQuery] Guid batterySystemId, [FromBody] BatteryCreationDTO dto)
        {
            Battery battery = new Battery();
            battery.BatterySystem = await _batteryService.GetBatterySystem(batterySystemId);
            battery.Capacity = dto.Capacity;
            battery = await _batteryService.CreateBattery(battery);
            return Ok(battery);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSolarPanelSystem([FromQuery] Guid smartHomeId, [FromBody] SmartDeviceDTO dto)
        {
            SolarPanelSystem solarPanelSystem = new SolarPanelSystem();
            solarPanelSystem.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            solarPanelSystem.Name = dto.Name;
            solarPanelSystem.Category = SmartDeviceCategory.VEU;
            solarPanelSystem = await _solarPanelService.CreateSolarPanelSystem(solarPanelSystem);
            return Ok(solarPanelSystem);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSolarPanel([FromQuery] Guid solarPanelSystemId, [FromBody] SolarPanelCreationDTO dto)
        {
            SolarPanel solarPanel = new SolarPanel();
            solarPanel.solarPanelSystem = await _solarPanelService.GetSolarPanelSystem(solarPanelSystemId);
            solarPanel.Area = dto.Area;
            solarPanel.Efficiency = dto.Efficiency;
            solarPanel = await _solarPanelService.CreateSolarPanel(solarPanel);
            return Ok(solarPanel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicleCharger([FromQuery] Guid smartHomeId, [FromBody] VehicleChargerCreationDTO dto)
        {
            VehicleCharger vehicleCharger = new VehicleCharger();
            vehicleCharger.SmartHome = await _smartHomeService.GetSmartHome(smartHomeId);
            vehicleCharger.Name = dto.Name;
            vehicleCharger.Category = SmartDeviceCategory.VEU;
            vehicleCharger.Power = dto.Power;
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
