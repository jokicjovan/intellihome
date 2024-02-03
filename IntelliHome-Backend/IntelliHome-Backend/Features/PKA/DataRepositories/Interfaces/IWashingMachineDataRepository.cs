using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces
{
    public interface IWashingMachineDataRepository
    {
        void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
        List<WashingMachineData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        WashingMachineData GetLastData(Guid id);
        List<WashingMachineData> GetLastHourData(Guid id);
    }
}
