using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IBatterySystemService : ICrudService<BatterySystem>
    {
        List<BatterySystemDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task<BatterySystemDTO> GetWithData(Guid id);
    }
}
