using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface ISolarPanelSystemService : ICrudService<SolarPanelSystem>
    {
        void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
        Task<SolarPanelSystemDTO> GetWithProductionData(Guid id);
        Task Toggle(Guid id, String togglerUsername, bool turnOn = true);
    }
}
