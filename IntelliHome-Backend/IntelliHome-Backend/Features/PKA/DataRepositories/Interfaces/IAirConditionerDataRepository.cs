using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces
{
    public interface IAirConditionerDataRepository
    {
        void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
        List<AirConditionerData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        AirConditionerData GetLastData(Guid id);
        List<AirConditionerData> GetLastHourData(Guid id);
    }
}
