using Data.Models.Home;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ICityService : ICrudService<City>
    {
        Task<CityPaginatedDTO> GetAllPaged(String search, PageParametersDTO pageParameters);
        Task<List<SmartHomeUsageDataDTO>> GetUsageHistoricalData(Guid id, DateTime from, DateTime to);
    }
}
