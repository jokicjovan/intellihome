using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class VehicleChargerService : IVehicleChargerService
    {
        private readonly IVehicleChargerRepository _vehicleChargerRepository;
        private readonly IVehicleChargingPointRepository _vehicleChargingPointRepository;

        public VehicleChargerService(IVehicleChargerRepository vehicleChargerRepository, IVehicleChargingPointRepository vehicleChargingPointRepository)
        {
            _vehicleChargerRepository = vehicleChargerRepository;
            _vehicleChargingPointRepository = vehicleChargingPointRepository;
        }

        public Task<VehicleCharger> CreateVehicleCharger(VehicleCharger vehicleCharger)
        {
            return _vehicleChargerRepository.Create(vehicleCharger);
        }

        public async Task<VehicleCharger> GetVehicleCharger(Guid Id)
        {
            VehicleCharger vehicleCharger = await _vehicleChargerRepository.Read(Id);
            if (vehicleCharger == null)
            {
                throw new ResourceNotFoundException("Vehicle charger with provided Id not found!");
            }
            return vehicleCharger;
        }

        public Task<VehicleChargingPoint> CreateVehicleChargingPoint(VehicleChargingPoint vehicleChargingPoint)
        {
            return _vehicleChargingPointRepository.Create(vehicleChargingPoint);
        }
    }
}
