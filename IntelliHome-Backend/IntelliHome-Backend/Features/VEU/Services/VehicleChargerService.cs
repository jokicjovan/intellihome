using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class VehicleChargerService : IVehicleChargerService
    {
        private readonly IVehicleChargerRepository _vehicleChargerRepository;
        private readonly IVehicleChargingPointRepository _vehicleChargingPointRepository;
        private readonly IVehicleChargerDataRepository _vehicleChargerDataRepository;
        private readonly IVehicleChargerHandler _vehicleChargerHandler;

        public VehicleChargerService(IVehicleChargerRepository vehicleChargerRepository, IVehicleChargingPointRepository vehicleChargingPointRepository,
            IVehicleChargerHandler vehicleChargerHandler, IVehicleChargerDataRepository vehicleChargerDataRepository)
        {
            _vehicleChargerRepository = vehicleChargerRepository;
            _vehicleChargingPointRepository = vehicleChargingPointRepository;
            _vehicleChargerHandler = vehicleChargerHandler;
            _vehicleChargerDataRepository = vehicleChargerDataRepository;
        }

        public async Task<VehicleCharger> Create(VehicleCharger entity)
        {
            entity = await _vehicleChargerRepository.Create(entity);
            bool success = await _vehicleChargerHandler.ConnectToSmartDevice(entity);
            if (success)
            {
                entity.IsConnected = true;
                await _vehicleChargerRepository.Update(entity);
            }
            return entity;
        }

        public async Task Toggle(Guid id, String togglerUsername,  bool turnOn = true)
        {
            VehicleCharger vehicleCharger = await _vehicleChargerRepository.FindWithSmartHome(id);
            if (vehicleCharger == null)
            {
                throw new ResourceNotFoundException("Smart device not found!");
            }
            await _vehicleChargerHandler.ToggleSmartDevice(vehicleCharger, turnOn);
            vehicleCharger.IsOn = turnOn;
            _ = _vehicleChargerRepository.Update(vehicleCharger);

            var fields = new Dictionary<string, object>
            {
                { "action", turnOn ? "ON" : "OFF" }

            };
            var tags = new Dictionary<string, string>
            {
                { "actionBy", togglerUsername},
                { "deviceId", id.ToString()}
            };
            _vehicleChargerDataRepository.AddActionMeasurement(fields, tags);
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
