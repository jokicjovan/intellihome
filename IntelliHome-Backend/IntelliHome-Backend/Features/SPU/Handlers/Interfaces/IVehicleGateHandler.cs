using Data.Models.SPU;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Handlers.Interfaces
{
    public interface IVehicleGateHandler : ISmartDeviceHandler
    {
        void ChangeMode(VehicleGate vehicle, bool isPublic);
    }
}
