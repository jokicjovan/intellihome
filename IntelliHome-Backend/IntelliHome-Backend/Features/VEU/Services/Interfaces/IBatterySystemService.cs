using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.BatterySystem;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IBatterySystemService : ICrudService<BatterySystem>
    {
        List<BatterySystemCapacityDataDTO> GetCapacityHistoricalData(Guid id, DateTime from, DateTime to);
        void AddCapacityMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task<BatterySystemDTO> GetWithCapacityData(Guid id);
        Task Toggle(Guid id, bool turnOn);
        Task<BatterySystem> GetWithHome(Guid id);
    }
}
