using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Infrastructure;
using IntelliHome_Backend.Features.Shared.Services.Interfacted;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        private readonly IDataChangeListener _dataChangeListener;

        public PKAController(ISmartHomeService smartHomeService, IAirConditionerService airConditionerService,
            IAmbientSensorService ambientSensorService, IWashingMachineService washingMachineService,
            IImageService imageService, IDataChangeListener dataChangeListener)
        {
            _smartHomeService = smartHomeService;
            _airConditionerService = airConditionerService;
            _ambientSensorService = ambientSensorService;
            _washingMachineService = washingMachineService;
            _imageService = imageService;
            _dataChangeListener = dataChangeListener;
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateAirConditioner([FromRoute] Guid smartHomeId, [FromForm] AirConditionerCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
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
            _dataChangeListener.HandleDataChange(smartHomeId.ToString() + " " + username);
            return Ok(airConditioner);
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateAmbientSensor([FromRoute] Guid smartHomeId, [FromForm] AmbientSensorCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
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
            _dataChangeListener.HandleDataChange(smartHomeId.ToString() + " " + username);
            return Ok(ambientSensor);
        }

        [HttpPost]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> CreateWashingMachine([FromRoute] Guid smartHomeId, [FromForm] WashingMachineCreationDTO dto)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name).Value;
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
            _dataChangeListener.HandleDataChange(smartHomeId.ToString() + " " + username);
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
