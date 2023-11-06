using Data.Models.Home;
using Data.Models.PKA;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace IntelliHome_Backend.Features.Home
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmartHomeController : ControllerBase
    {
        private readonly ISmartHomeService _smartHomeService;

        public SmartHomeController(ISmartHomeService smartHomeService)
        {
            _smartHomeService = smartHomeService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSmartHome([FromBody] SmartHomeCreationDTO dto)
        {
            // TODO: Get user from token
            GetSmartHomeDTO smartHome;
            try
            {
                smartHome = await _smartHomeService.CreateSmartHome(dto, "boki");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(smartHome);
        }

        [HttpGet]
        public async Task<ActionResult> GetSmartHome(Guid Id)
        {
            GetSmartHomeDTO smartHome;
            try
            {
                smartHome = await _smartHomeService.GetSmartHomeDTO(Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(smartHome);
        }

        [HttpGet]
        public async Task<ActionResult> GetSmartHomesForUser()
        {
            // TODO: Get user from token
            List<GetSmartHomeDTO> smartHomes;
            try
            {
                smartHomes = await _smartHomeService.GetSmartHomesForUser("boki");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(smartHomes);
        }


        //TODO: Add authorization, just admin can approve smart homes
        [HttpPut]
        public async Task<ActionResult> ApproveSmartHome(Guid id)
        {
            try
            {
                await _smartHomeService.ApproveSmartHome(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Smart home approved!");
        }


        // TODO: Add authorization, just admin can delete smart homes or owner of the smart home
        [HttpDelete]
        public async Task<ActionResult> DeleteSmartHome(Guid id)
        {
            try
            {
                await _smartHomeService.DeleteSmartHome(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Smart home deleted!");
        }
    }
}