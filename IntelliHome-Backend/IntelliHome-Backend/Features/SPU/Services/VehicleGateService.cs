using Data.Models.SPU;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class VehicleGateService : IVehicleGateService
    {
        private readonly IVehicleGateRepository _vehicleGateRepository;
        private readonly IDeviceConnectionService _deviceConnectionService;

        public VehicleGateService(IVehicleGateRepository vehicleGateRepository, IDeviceConnectionService deviceConnectionService)
        {
            _vehicleGateRepository = vehicleGateRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<VehicleGate> CreateVehicleGate(VehicleGate vehicleGate)
        {
            vehicleGate = await _vehicleGateRepository.Create(vehicleGate);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(vehicleGate);
            if (success)
            {
                vehicleGate.IsConnected = true;
                await _vehicleGateRepository.Update(vehicleGate);
            }
            return vehicleGate;
        }
    }
}
