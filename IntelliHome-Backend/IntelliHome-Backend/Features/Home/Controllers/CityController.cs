using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Services;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntelliHome_Backend.Features.Home.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCitiesPaged([FromQuery] PageParametersDTO pageParameters, [FromQuery] string search)
        {
            if (search == null)
            {
                search = "";
            }
            search = search.Substring(1, search.Length - 2);
            CityPaginatedDTO result = await _cityService.GetAllPaged(search, pageParameters);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCityHistoricalData(Guid id, DateTime from, DateTime to)
        {
            if (from > to)
            {
                return BadRequest("FROM date cant be after TO date");
            }
            List<SmartHomeUsageDataDTO> result = await _cityService.GetUsageHistoricalData(id, from, to);
            return Ok(result);
        }
    }
}
