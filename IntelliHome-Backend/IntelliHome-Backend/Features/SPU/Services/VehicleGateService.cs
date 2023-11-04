using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class VehicleGateService : IVehicleGateService
    {
        private readonly IVehicleGateRepository _vehicleGateRepository;
        public VehicleGateService(IVehicleGateRepository vehicleGateRepository)
        {
            _vehicleGateRepository = vehicleGateRepository;
        }

        public Task<VehicleGate> CreateVehicleGate(VehicleGate vehicleGate)
        {
            return _vehicleGateRepository.Create(vehicleGate);
        }
    }
}
