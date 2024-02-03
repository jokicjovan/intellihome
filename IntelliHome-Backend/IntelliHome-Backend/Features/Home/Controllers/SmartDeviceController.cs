using Data.Models.Home;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Services;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntelliHome_Backend.Features.Home.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmartDeviceController : ControllerBase
    {
        private readonly ISmartDeviceService _smartDeviceService;
        private readonly IUserService _userService;

        public SmartDeviceController(ISmartDeviceService smartDeviceService,IUserService userService)
        {
            _smartDeviceService = smartDeviceService;
            _userService = userService;
        }

        [HttpGet]
        [Route("{smartHomeId:Guid}")]
        public async Task<ActionResult> GetSmartDevicesForHome([FromRoute] Guid smartHomeId, [FromQuery] PageParametersDTO pageParameters)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            (IEnumerable<SmartDeviceDTO>, int) resultTuple = await _smartDeviceService.GetPagedSmartDevicesForSmartHome(smartHomeId, pageParameters.PageNumber, pageParameters.PageSize,Guid.Parse(id));
            SmartDevicesPaginatedDTO dto = new SmartDevicesPaginatedDTO
            {
                SmartDevices = resultTuple.Item1,
                TotalCount = resultTuple.Item2
            };
            return Ok(dto);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetSharedList(string deviceId)
        {
            List<String> emails = new List<string>();
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Data.Models.Users.User user = await _userService.Get(Guid.Parse(id));
            SmartDevice smartDevice = await _smartDeviceService.Get(Guid.Parse(deviceId));
            if (user.Id.Equals(smartDevice.SmartHome.Owner.Id))
            {
                emails=await _smartDeviceService.GetSharedListUser(smartDevice);
            }
            else
            {
                return BadRequest("Permission Denied");
            }

            return Ok(emails);
        }


        [HttpPut]
        [Authorize]
        public async Task<ActionResult> AddPermission([FromBody] PermissionDTO permission)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Data.Models.Users.User user = await _userService.Get(Guid.Parse(id));
            SmartDevice smartDevice = await _smartDeviceService.Get(Guid.Parse(permission.home));
            if (user.Id.Equals(smartDevice.SmartHome.Owner.Id))
            {
                await _smartDeviceService.AddPermision(smartDevice, permission.user);
            }
            else
            {
                return BadRequest("Permission Denied");
            }

            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> RemovePermission([FromBody] PermissionDTO permission)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("Cookie error");
            }
            ClaimsIdentity identity = result.Principal.Identity as ClaimsIdentity;

            String id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            String passwordChanged = null;
            Data.Models.Users.User user = await _userService.Get(Guid.Parse(id));
            SmartDevice smartDevice = await _smartDeviceService.Get(Guid.Parse(permission.home));
            if (user.Id.Equals(smartDevice.SmartHome.Owner.Id))
            {
                await _smartDeviceService.RemovePermision(smartDevice, permission.user);
            }
            else
            {
                return BadRequest("Permission Denied");
            }

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> TurnOnSmartDevice(Guid id, bool turnOn)
        {
            await _smartDeviceService.TurnOnSmartDevice(id, turnOn);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetAvailabilityData(Guid id, string h)
        {
            List<AvailabilityData> result = _smartDeviceService.GetAvailabilityData(id, h);
            return Ok(result);
        }

    }
}
