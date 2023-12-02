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

        public async Task<VehicleCharger> Create(VehicleCharger entity)
        {
            entity = await _vehicleChargerRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _vehicleChargerRepository.Update(entity);
            //}
            return entity;
        }

        public Task<VehicleCharger> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<VehicleCharger> Get(Guid id)
        {
            VehicleCharger vehicleCharger = await _vehicleChargerRepository.Read(id);
            if (vehicleCharger == null)
            {
                throw new ResourceNotFoundException("Vehicle charger with provided Id not found!");
            }
            return vehicleCharger;
        }

        public Task<IEnumerable<VehicleCharger>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<VehicleCharger> Update(VehicleCharger entity)
        {
            throw new NotImplementedException();
        }
    }
}
