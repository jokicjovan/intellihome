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

        public VEUController(ISmartHomeService smartHomeService, IBatteryService batteryService)
        {
            _smartHomeService = smartHomeService;
            _batteryService = batteryService;
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
        public async Task<ActionResult> CreateBattery([FromQuery] Guid BatterySystemId, [FromBody] BatteryCreationDTO dto)
        {
            Battery battery = new Battery();
            battery.Capacity = dto.Capacity;
            battery = await _batteryService.AddBatteryToSystem(BatterySystemId, battery);
            return Ok(battery);
        }
    }
}
