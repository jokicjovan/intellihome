using Data.Models.SPU;

namespace IntelliHome_Backend.Features.SPU.Services.Interfaces
{
    public interface IVehicleGateService
    {
        Task<VehicleGate> CreateVehicleGate(VehicleGate vehicleGate);
    }
}
