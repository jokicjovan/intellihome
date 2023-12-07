using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces
{
    public interface IBatterySystemDataRepository
    {
        List<BatterySystemDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        BatterySystemDataDTO GetLastData(Guid id);
    }
}
