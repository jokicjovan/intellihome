using Data.Models.VEU;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        private readonly IHubContext<SmartDeviceHub, ISmartDeviceClient> _smartDeviceHubContext;

        public VehicleChargerService(IVehicleChargerRepository vehicleChargerRepository, IVehicleChargingPointRepository vehicleChargingPointRepository,
            IVehicleChargerHandler vehicleChargerHandler, IVehicleChargerDataRepository vehicleChargerDataRepository, IVehicleChargingPointDataRepository vehicleChargingPointDataRepository,
            ISmartDeviceDataRepository smartDeviceDataRepository, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
        {
            _vehicleChargerRepository = vehicleChargerRepository;
            _vehicleChargingPointRepository = vehicleChargingPointRepository;
            _vehicleChargerHandler = vehicleChargerHandler;
            _vehicleChargerDataRepository = vehicleChargerDataRepository;
            _vehicleChargingPointDataRepository = vehicleChargingPointDataRepository;
            _smartDeviceDataRepository = smartDeviceDataRepository;
            _smartDeviceHubContext = smartDeviceHubContext;
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
            return _vehicleChargerRepository.Update(entity);
        }

        public Task<VehicleChargingPoint> Update(VehicleChargingPoint entity)
        {
            return _vehicleChargingPointRepository.Update(entity);
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
                    StartTime = vehicleChargingPoint.StartTime,
                    EndTime = vehicleChargingPoint.EndTime,
                    Status = vehicleChargingPoint.Status,
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
            SaveActionAndInformUsers(turnOn ? "ON" : "OFF", togglerUsername, id.ToString());
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

            // if (!vehicleChargingPoint.IsFree) {
            //     throw new ResourceNotFoundException("Vehicle charging point is not free!");
            // }

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
            SaveActionAndInformUsers("CHARGER CONNECTED", "SYSTEM", vehicleChargerId.ToString());
            return vehicleCharger;
        }

        public async Task<VehicleCharger> DisconnectFromCharger(Guid vehicleChargerId, Guid vehicleChargingPointId) {
            VehicleCharger vehicleCharger = await GetWithHome(vehicleChargerId);
            var vehicleChargingPoint = vehicleCharger.ChargingPoints.FirstOrDefault(e => e.Id == vehicleChargingPointId);

            if (vehicleChargingPoint == null)
            {
                throw new ResourceNotFoundException("Vehicle charging point with provided Id does not exist on charger!");
            }

            if (vehicleChargingPoint.IsFree)
            {
                throw new ResourceNotFoundException("Vehicle charging point is free!");
            }

            vehicleChargingPoint.IsFree = true;
            vehicleChargingPoint.ChargeLimit = null;
            vehicleChargingPoint.Capacity = null;
            vehicleChargingPoint.InitialCapacity = null;
            vehicleChargingPoint.Status = null;
            vehicleChargingPoint.StartTime = null;
            vehicleChargingPoint.EndTime = null;
            vehicleCharger = await _vehicleChargerRepository.Update(vehicleCharger);

            Dictionary<string, object> payload = new Dictionary<string, object>
            {
                { "action", "chargingPointDisconnected" },
                { "chargingPointId", vehicleChargingPoint.Id }
            };
            await _vehicleChargerHandler.PublishMessageToSmartDevice(vehicleCharger, JsonConvert.SerializeObject(payload));
            SaveActionAndInformUsers("CHARGER DISCONNECTED", "SYSTEM", vehicleChargerId.ToString());
            return vehicleCharger;
        }

        public void SaveActionAndInformUsers(String action, String actionBy, String deviceId) {
            var fields = new Dictionary<string, object>
            {
                { "action", action }

            };
            var tags = new Dictionary<string, string>
            {
                { "actionBy", actionBy},
                { "deviceId", deviceId}
            };
            AddActionMeasurement(fields, tags);

            ActionDataDTO actionDataDto = new()
            {
                Action = action,
                ActionBy = actionBy,
                Timestamp = DateTime.Now,
            };
            _ = _smartDeviceHubContext.Clients.Group(deviceId).ReceiveSmartDeviceData(JsonConvert.SerializeObject(actionDataDto, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));
        }
    }
}
