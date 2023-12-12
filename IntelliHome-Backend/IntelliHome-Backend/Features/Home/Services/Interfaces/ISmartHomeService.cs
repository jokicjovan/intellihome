using Data.Models.Home;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ISmartHomeService : ICrudService<SmartHome>
    {
        Task<GetSmartHomeDTO> GetSmartHomeDTO(Guid Id);
        Task<GetSmartHomeDTO> CreateSmartHome(SmartHomeCreationDTO dto, String username);
        Task<SmartHomePaginatedDTO> GetSmartHomesForUser(String username, String search, PageParametersDTO pageParameters);
        Task ApproveSmartHome(Guid id);
        Task DeleteSmartHome(Guid id, Guid userId, String reason);
        Task<SmartHomePaginatedDTO> GetSmartHomesForApproval(PageParametersDTO pageParameters);
        Task<bool> IsUserAllowed(Guid smartHomeId, Guid userId);
        List<SmartHomeUsageDataDTO> GetUsageHistoricalData(Guid id, DateTime from, DateTime to);
        void AddUsageMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
    }
}
