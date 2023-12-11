using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.Home.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmartDeviceController : ControllerBase
    {
        private readonly ISmartDeviceService _smartDeviceService;

        public SmartDeviceController(ISmartDeviceService smartDeviceService)
        {
            _smartDeviceService = smartDeviceService;
        }

        [HttpGet]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> GetSmartDevicesForHome([FromRoute] Guid smartHomeId, [FromQuery] PageParametersDTO pageParameters)
        {
            (IEnumerable<SmartDevice>, int) resultTuple = await _smartDeviceService.GetPagedSmartDevicesForSmartHome(smartHomeId, pageParameters.PageNumber, pageParameters.PageSize);
            SmartDevicesPaginatedDTO dto = new SmartDevicesPaginatedDTO
            {
                SmartDevices = resultTuple.Item1,
                TotalCount = resultTuple.Item2
            };
            return Ok(dto);
        }

        [HttpPut]
        public async Task<ActionResult> TurnOnSmartDevice(Guid id, bool turnOn)
        {
            await _smartDeviceService.TurnOnSmartDevice(id, turnOn);
            return Ok();
        }

    }
}
