using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface ISolarPanelSystemService : ICrudService<SolarPanelSystem>
    {
        void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to);
        Task<SolarPanelSystemDTO> GetWithProductionData(Guid id);
        void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
        Task Toggle(Guid id, String togglerUsername, bool turnOn = true);
        Task<SolarPanelSystem> GetWithHome(Guid id);
        void SaveActionAndInformUsers(String action, String actionBy, String deviceId);
    }
}
