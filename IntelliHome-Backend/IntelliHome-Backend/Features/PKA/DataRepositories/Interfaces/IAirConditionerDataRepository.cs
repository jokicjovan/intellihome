using IntelliHome_Backend.Features.PKA.DTOs;

namespace IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces
{
    public interface IAirConditionerDataRepository
    {
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<AirConditionerData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        AirConditionerData GetLastData(Guid id);
        List<AirConditionerData> GetLastHourData(Guid id);
    }
}
