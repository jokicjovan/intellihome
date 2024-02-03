using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class CityPaginatedDTO
    {
        public IEnumerable<CityDTO> Cities { get; set; }
        public Int32 TotalCount { get; set; }
    }
}
