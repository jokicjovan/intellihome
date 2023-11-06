using Data.Models.VEU;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IVehicleChargerService
    {
        Task<VehicleCharger> CreateVehicleCharger(VehicleCharger vehicleCharger);
        Task<VehicleCharger> GetVehicleCharger(Guid Id);
        Task<VehicleChargingPoint> CreateVehicleChargingPoint(VehicleChargingPoint vehicleChargingPoint);
    }
}
