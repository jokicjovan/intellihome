using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface ISolarPanelSystemService : ICrudService<SolarPanelSystem>
    {
        List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to);
        void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task<SolarPanelSystemDTO> GetWithProductionData(Guid id);
    }
}
