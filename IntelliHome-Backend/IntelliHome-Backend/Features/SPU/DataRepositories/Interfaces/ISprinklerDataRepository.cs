using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces
{
    public interface ISprinklerDataRepository
    {
        List<SprinklerData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        List<SprinklerData> GetLastHourData(Guid id);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        SprinklerData GetLastData(Guid id);
        void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
    }
}
