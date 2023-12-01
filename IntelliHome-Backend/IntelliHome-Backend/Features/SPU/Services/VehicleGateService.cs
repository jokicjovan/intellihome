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

        public async Task<VehicleGate> Create(VehicleGate entity)
        {
            entity = await _vehicleGateRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _vehicleGateRepository.Update(entity);
            //}
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
