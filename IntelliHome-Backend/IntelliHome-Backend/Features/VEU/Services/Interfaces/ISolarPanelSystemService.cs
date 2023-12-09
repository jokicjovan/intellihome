using Data.Models.PKA;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface ISolarPanelSystemService : ICrudService<SolarPanelSystem>
    {
        List<SolarPanelSystemDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task<SolarPanelSystemDTO> GetWithData(Guid id);
    }
}
