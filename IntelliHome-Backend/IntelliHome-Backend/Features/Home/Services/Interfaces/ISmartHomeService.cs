using Data.Models.Home;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

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
    }
}
