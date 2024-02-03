using IntelliHome_Backend.Features.PKA.DTOs;

namespace IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces
{
    public interface IAmbientSensorDataRepository
    {
        List<AmbientSensorData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        AmbientSensorData GetLastData(Guid id);
        List<AmbientSensorData> GetLastHourData(Guid id);
    }
}
