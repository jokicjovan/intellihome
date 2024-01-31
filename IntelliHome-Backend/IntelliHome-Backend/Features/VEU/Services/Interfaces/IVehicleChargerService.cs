using Data.Models.PKA;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IVehicleChargerService : ICrudService<VehicleCharger>
    {
        Task<VehicleChargingPoint> Update(VehicleChargingPoint entity);
        Task<VehicleCharger> GetWithHome(Guid id);
        Task<VehicleChargerDTO> GetWithChargingPointsData(Guid id);
        void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
        Task Toggle(Guid id, String togglerUsername, bool turnOn = true);
        void AddVehicleChargingPointMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<VehicleChargingPointDataDTO> GetVehicleChargingPointHistoricalData(Guid id, DateTime from, DateTime to);
        Task<VehicleCharger> ConnectToCharger(Guid vehicleChargerId, VehicleChargingPoint vehicleChargingPoint);
        Task<VehicleCharger> DisconnectFromCharger(Guid vehicleChargerId, Guid vehicleChargingPointId);
        void SaveActionAndInformUsers(String action, String actionBy, String deviceId);
    }
}
