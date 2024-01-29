using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;

namespace IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces
{
    public interface IVehicleChargingPointDataRepository
    {
        void AddVehicleChargingPointMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<VehicleChargingPointDataDTO> GetVehicleChargingPointHistoricalData(Guid id, DateTime from, DateTime to);
        VehicleChargingPointDataDTO GetLastVehicleChargingPointData(Guid id);
    }
}
