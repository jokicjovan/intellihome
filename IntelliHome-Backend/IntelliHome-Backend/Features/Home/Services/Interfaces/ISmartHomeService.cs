using Data.Models.Home;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ISmartHomeService
    {
        Task<SmartHome> GetSmartHome(Guid Id);
        Task<GetSmartHomeDTO> GetSmartHomeDTO(Guid Id);
        Task<GetSmartHomeDTO> CreateSmartHome(SmartHomeCreationDTO dto, String username);
        Task<SmartHomePaginatedDTO> GetSmartHomesForUser(string username, PageParametersDTO pageParameters);
        Task ApproveSmartHome(Guid id);
        Task DeleteSmartHome(Guid id);
    }
}
