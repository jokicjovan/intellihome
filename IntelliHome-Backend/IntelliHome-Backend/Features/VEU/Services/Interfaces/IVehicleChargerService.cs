using Data.Models.PKA;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IVehicleChargerService : ICrudService<VehicleCharger>
    {
        Task Toggle(Guid id, String togglerUsername, bool turnOn = true);
    }
}
