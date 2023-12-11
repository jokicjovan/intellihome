using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces
{
    public interface IBatterySystemDataRepository
    {
        List<BatterySystemCapacityDataDTO> GetCapacityHistoricalData(Guid id, DateTime from, DateTime to);
        void AddCapacityMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        BatterySystemCapacityDataDTO GetLastCapacityData(Guid id);
    }
}
