using Data.Models.VEU;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class VehicleChargerService : IVehicleChargerService
    {
        private readonly IVehicleChargerRepository _vehicleChargerRepository;
        private readonly IVehicleChargingPointRepository _vehicleChargingPointRepository;
        private readonly IDeviceConnectionService _deviceConnectionService;

        public VehicleChargerService(IVehicleChargerRepository vehicleChargerRepository, IVehicleChargingPointRepository vehicleChargingPointRepository, IDeviceConnectionService deviceConnectionService)
        {
            _vehicleChargerRepository = vehicleChargerRepository;
            _vehicleChargingPointRepository = vehicleChargingPointRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<VehicleCharger> CreateVehicleCharger(VehicleCharger vehicleCharger)
        {
            vehicleCharger = await _vehicleChargerRepository.Create(vehicleCharger);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(vehicleCharger);
            if (success)
            {
                vehicleCharger.IsConnected = true;
                await _vehicleChargerRepository.Update(vehicleCharger);
            }
            return vehicleCharger;
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
    }
}
