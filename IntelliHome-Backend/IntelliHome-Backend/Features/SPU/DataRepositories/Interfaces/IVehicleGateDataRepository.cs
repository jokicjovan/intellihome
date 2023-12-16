using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces
{
    public interface IVehicleGateDataRepository 
    {
        VehicleGateData GetLastData(Guid id);
        List<VehicleGateData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        void SaveAction(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<ActionDataDTO> GetHistoricalActionData(Guid id, DateTime from, DateTime to);
    }
}
