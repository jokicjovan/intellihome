using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.VEU.DTOs.BatterySystem;

namespace IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces
{
    public interface IBatterySystemDataRepository
    {
        void AddCapacityMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<BatterySystemCapacityDataDTO> GetCapacityHistoricalData(Guid id, DateTime from, DateTime to);
        BatterySystemCapacityDataDTO GetLastCapacityData(Guid id);
    }
}
