using Data.Models.VEU;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Newtonsoft.Json;
using System;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class VehicleChargerService : IVehicleChargerService
    {
        private readonly IVehicleChargerRepository _vehicleChargerRepository;
        private readonly IVehicleChargerDataRepository _vehicleChargerDataRepository;
        private readonly IVehicleChargerHandler _vehicleChargerHandler;
        private readonly IVehicleChargingPointRepository _vehicleChargingPointRepository;
        private readonly IVehicleChargingPointDataRepository _vehicleChargingPointDataRepository;
        private readonly ISmartDeviceDataRepository _smartDeviceDataRepository;

        public VehicleChargerService(IVehicleChargerRepository vehicleChargerRepository, IVehicleChargingPointRepository vehicleChargingPointRepository,
            IVehicleChargerHandler vehicleChargerHandler, IVehicleChargerDataRepository vehicleChargerDataRepository, IVehicleChargingPointDataRepository vehicleChargingPointDataRepository,
            ISmartDeviceDataRepository smartDeviceDataRepository)
        {
            _vehicleChargerRepository = vehicleChargerRepository;
            _vehicleChargingPointRepository = vehicleChargingPointRepository;
            _vehicleChargerHandler = vehicleChargerHandler;
            _vehicleChargerDataRepository = vehicleChargerDataRepository;
            _vehicleChargingPointDataRepository = vehicleChargingPointDataRepository;
            _smartDeviceDataRepository = smartDeviceDataRepository;
        }

        public async Task<VehicleCharger> Create(VehicleCharger entity)
        {
            entity = await _vehicleChargerRepository.Create(entity);
            bool success = await _vehicleChargerHandler.ConnectToSmartDevice(entity);
            if (!success) return entity;
            entity.IsConnected = true;
            entity = await _vehicleChargerRepository.Update(entity);

            var fields = new Dictionary<string, object>
            {
                { "isConnected", 1 }

            };
            var tags = new Dictionary<string, string>
            {
                { "deviceId", entity.Id.ToString()}
            };
            _smartDeviceDataRepository.AddPoint(fields, tags);

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

        public async Task<VehicleCharger> GetWithHome(Guid id)
        {
            VehicleCharger vehicleCharger = await _vehicleChargerRepository.FindWithSmartHome(id);
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

        public async Task<VehicleChargerDTO> GetWithChargingPointsData(Guid id)
        {
            VehicleCharger vehicleCharger = await GetWithHome(id);
            VehicleChargerDTO vehicleChargerDTO = new VehicleChargerDTO
            {
                Id = vehicleCharger.Id,
                Name = vehicleCharger.Name,
                IsConnected = vehicleCharger.IsConnected,
                IsOn = vehicleCharger.IsOn,
                Category = vehicleCharger.Category.ToString(),
                Type = vehicleCharger.Type.ToString(),
                SmartHomeId = vehicleCharger.SmartHome.Id,
                PowerPerHour = vehicleCharger.PowerPerHour,
            };

            foreach (VehicleChargingPoint vehicleChargingPoint in vehicleCharger.ChargingPoints) {
                VehicleChargingPointDataDTO vehicleChargingPointDataDTO = _vehicleChargingPointDataRepository.GetLastVehicleChargingPointData(vehicleChargingPoint.Id);
                VehicleChargingPointDTO vehicleChargingPointDTO = new VehicleChargingPointDTO
                {
                    Id = vehicleChargingPoint.Id,
                    ChargeLimit = vehicleChargingPoint.ChargeLimit,
                    InitialCapacity = vehicleChargingPoint.InitialCapacity,
                    Capacity = vehicleChargingPoint.Capacity,
                    CurrentCapacity = vehicleChargingPointDataDTO.CurrentCapacity,
                    StartTime = vehicleChargingPointDataDTO.StartTime,
                    EndTime = vehicleChargingPointDataDTO.EndTime,
                    Status = vehicleChargingPointDataDTO.Status,
                    IsFree = vehicleChargingPoint.IsFree,
                };
                vehicleChargerDTO.ChargingPoints.Add(vehicleChargingPointDTO);
            }

            return vehicleChargerDTO;
        }

        public void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _vehicleChargerDataRepository.AddActionMeasurement(fields, tags);
        }

        public void AddVehicleChargingPointMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _vehicleChargingPointDataRepository.AddVehicleChargingPointMeasurement(fields, tags);
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
            AddActionMeasurement(fields, tags);
        }

        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _vehicleChargerDataRepository.GetActionHistoricalData(id, from, to);
        }
        public List<VehicleChargingPointDataDTO> GetVehicleChargingPointHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _vehicleChargingPointDataRepository.GetVehicleChargingPointHistoricalData(id, from, to);
        }

        public async Task<VehicleCharger> ConnectToCharger(Guid vehicleChargerId, VehicleChargingPoint newVehicleChargingPoint)
        {
            VehicleCharger vehicleCharger = await GetWithHome(vehicleChargerId);

            var vehicleChargingPoint = vehicleCharger.ChargingPoints.FirstOrDefault(e => e.Id == newVehicleChargingPoint.Id);

            if (vehicleChargingPoint == null) {
                throw new ResourceNotFoundException("Vehicle charging point with provided Id does not exist on charger!");
            }

            if (!vehicleChargingPoint.IsFree) {
                throw new ResourceNotFoundException("Vehicle charging point is not free!");
            }

            vehicleChargingPoint.IsFree = false;
            vehicleChargingPoint.ChargeLimit = newVehicleChargingPoint.ChargeLimit;
            vehicleChargingPoint.Capacity = newVehicleChargingPoint.Capacity;
            vehicleChargingPoint.InitialCapacity = newVehicleChargingPoint.InitialCapacity;
            vehicleCharger = await _vehicleChargerRepository.Update(vehicleCharger);

            Dictionary<string, object> payload = new Dictionary<string, object>
            {
                { "action", "chargingPointConnected" },
                { "chargingPointId", vehicleChargingPoint.Id.ToString()},
                { "capacity", vehicleChargingPoint.Capacity},
                { "currentCapacity", vehicleChargingPoint.InitialCapacity},
                { "chargeLimit", vehicleChargingPoint.ChargeLimit},
            };
            await _vehicleChargerHandler.PublishMessageToSmartDevice(vehicleCharger, JsonConvert.SerializeObject(payload));
            return vehicleCharger;
        }

        public async Task<VehicleCharger> DisconnectCharger(Guid vehicleChargerId, Guid vehicleChargingPointId) {
            VehicleCharger vehicleCharger = await GetWithHome(vehicleChargerId);
            foreach (VehicleChargingPoint candidate in vehicleCharger.ChargingPoints)
            {
                if (candidate.Id == vehicleChargingPointId)
                {
                    candidate.IsFree = true;
                    candidate.ChargeLimit = null;
                    candidate.Capacity = null;
                    candidate.InitialCapacity = null;
                    vehicleCharger = await _vehicleChargerRepository.Update(vehicleCharger);

                    Dictionary<string, object> payload = new Dictionary<string, object>
                    {
                        { "action", "chargingPointDisconnected" }
                    };
                    await _vehicleChargerHandler.PublishMessageToSmartDevice(vehicleCharger, JsonConvert.SerializeObject(payload));
                    return vehicleCharger;
                }
            }
            throw new ResourceNotFoundException("Vehicle charger with provided Id does not have charging point with provided Id!");
        }
    }
}
