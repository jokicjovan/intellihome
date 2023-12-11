using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces
{
    public interface ILampDataRepository
    {
        LampData GetLastData(Guid id);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<LampData> GetHistoricalData(Guid id, DateTime from, DateTime to);

    }
}
