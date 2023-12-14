using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces
{
    public interface IVehicleGateDataRepository 
    {
        VehicleGateData GetLastData(Guid id);
        List<VehicleGateData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
    }
}
