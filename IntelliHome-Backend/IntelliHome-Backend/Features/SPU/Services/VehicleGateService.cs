using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class VehicleGateService : IVehicleGateService
    {
        private readonly IVehicleGateRepository _vehicleGateRepository;
        private readonly IVehicleGateHandler _vehicleGateHandler;

        public VehicleGateService(IVehicleGateRepository vehicleGateRepository, IVehicleGateHandler vehicleGateHandler)
        {
            _vehicleGateRepository = vehicleGateRepository;
            _vehicleGateHandler = vehicleGateHandler;
        }

        public async Task<VehicleGate> Create(VehicleGate entity)
        {
            entity = await _vehicleGateRepository.Create(entity);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "is_public", entity.IsPublic },
                            { "allowed_licence_plates", entity.AllowedLicencePlates },
                            { "power_per_hour", entity.PowerPerHour }
                        };
            bool success = await _vehicleGateHandler.ConnectToSmartDevice(entity, additionalAttributes);
            if (success)
            {
                entity.IsConnected = true;
                await _vehicleGateRepository.Update(entity);
            }
            return entity;
        }

        public Task<VehicleGate> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleGate> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VehicleGate>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<VehicleGate> Update(VehicleGate entity)
        {
            throw new NotImplementedException();
        }
    }
}
