using Data.Models.SPU;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class VehicleGateService : IVehicleGateService
    {
        private readonly IVehicleGateRepository _vehicleGateRepository;
        private readonly IVehicleGateHandler _vehicleGateHandler;
        private readonly IVehicleGateDataRepository _vehicleGateDataRepository;
        private readonly IHubContext<SmartDeviceHub, ISmartDeviceClient> _smartDeviceHubContext;
        private readonly ISmartDeviceDataRepository _smartDeviceDataRepository;

        public VehicleGateService(
            IVehicleGateRepository vehicleGateRepository, 
            IVehicleGateHandler vehicleGateHandler, 
            IVehicleGateDataRepository vehicleGateDataRepository, 
            IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext, 
            ISmartDeviceDataRepository smartDeviceDataRepository)
        {
            _vehicleGateRepository = vehicleGateRepository;
            _vehicleGateHandler = vehicleGateHandler;
            _vehicleGateDataRepository = vehicleGateDataRepository;
            _smartDeviceHubContext = smartDeviceHubContext;
            _smartDeviceDataRepository = smartDeviceDataRepository;
        }

        public async Task<VehicleGate> Create(VehicleGate entity)
        {
            entity = await _vehicleGateRepository.Create(entity);
            bool success = await _vehicleGateHandler.ConnectToSmartDevice(entity);
            if (!success) return entity;
            entity.IsConnected = true;
            await _vehicleGateRepository.Update(entity);
            var fields = new Dictionary<string, object>
            {
                { "isConnected", true }

            };
            var tags = new Dictionary<string, string>
            {
                { "deviceId", entity.Id.ToString()}
            };
            _smartDeviceDataRepository.AddPoint(fields, tags);
            return entity;
        }

        

        public async Task<VehicleGateDTO> GetWithData(Guid id)
        {
            VehicleGate vehicleGate = await _vehicleGateRepository.FindWithSmartHome(id);
            VehicleGateDTO vehicleGateDTO = new VehicleGateDTO
            {
                Id = vehicleGate.Id,
                Name = vehicleGate.Name,
                IsConnected = vehicleGate.IsConnected,
                IsOn = vehicleGate.IsOn,
                Category = vehicleGate.Category.ToString(),
                Type = vehicleGate.Type.ToString(),
                SmartHomeId = vehicleGate.SmartHome.Id,
                PowerPerHour = vehicleGate.PowerPerHour,
                IsPublic = vehicleGate.IsPublic,
                AllowedLicencePlates = vehicleGate.AllowedLicencePlates,
            };

            VehicleGateData vehicleGateData = null;
            try
            {
                vehicleGateData = GetLastData(id);
            }
            catch (Exception)
            {
                vehicleGateData = null;
            }

            if (vehicleGateData != null)
            {
                vehicleGateDTO.CurrentLicencePlate = vehicleGateData.LicencePlate;
                vehicleGateDTO.IsEntering = vehicleGateData.IsEntering;
                vehicleGateDTO.IsOpen = vehicleGateData.IsOpen;
                vehicleGateDTO.IsOpenedByUser = vehicleGateData.IsOpenedByUser;
                vehicleGateDTO.ActionBy = vehicleGateData.ActionBy;
            }

            return vehicleGateDTO;
        }

        private VehicleGateData GetLastData(Guid id)
        {
            return _vehicleGateDataRepository.GetLastData(id);
        }

        public List<VehicleGateData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _vehicleGateDataRepository.GetHistoricalData(id, from, to);
        }

        public List<ActionDataDTO> GetHistoricalActionData(Guid id, DateTime from, DateTime to)
        {
            return _vehicleGateDataRepository.GetHistoricalActionData(id, from, to);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _vehicleGateDataRepository.AddPoint(fields, tags);
        }

        public void SaveAction(Dictionary<string, object> fields, Dictionary<string, string> tags, Guid id)
        {
            //concat fields and tags
            ActionDataDTO actionDataDto = new()
            {
                Action = fields["action"].ToString(),
                ActionBy = tags["username"],
                Timestamp = DateTime.Now,
            };
            _smartDeviceHubContext.Clients.Group(id.ToString()).ReceiveSmartDeviceData(JsonConvert.SerializeObject(actionDataDto));
            _vehicleGateDataRepository.SaveAction(fields, tags);
        }

        public async Task AddLicencePlate(Guid id, string licencePlate)
        {
            VehicleGate vehicleGate = await _vehicleGateRepository.FindWithSmartHome(id);
            vehicleGate.AllowedLicencePlates.Add(licencePlate);
            await _vehicleGateRepository.Update(vehicleGate);

            _vehicleGateHandler.AddLicencePlate(vehicleGate, licencePlate);
        }

        public async Task RemoveLicencePlate(Guid id, string licencePlate)
        {
            VehicleGate vehicleGate = await _vehicleGateRepository.FindWithSmartHome(id);
            vehicleGate.AllowedLicencePlates.Remove(licencePlate);
            await _vehicleGateRepository.Update(vehicleGate);

            _vehicleGateHandler.RemoveLicencePlate(vehicleGate, licencePlate);
        }

        public async Task OpenCloseGate(Guid id, bool isOpen, string username)
        {
            VehicleGate vehicleGate = await _vehicleGateRepository.FindWithSmartHome(id);
            _vehicleGateHandler.OpenCloseGate(vehicleGate, isOpen, username);

            string action = isOpen ? "open_gate" : "close_gate";

            var fields = new Dictionary<string, object>
            {
                { "action", action }

            };
            var tags = new Dictionary<string, string>
            {
                { "username", username},
                { "deviceId", id.ToString()}
            };

            SaveAction(fields, tags, id);
        }

        

        public async Task ChangeMode(Guid id, bool isPublic, string username)
        {
            VehicleGate vehicleGate = await _vehicleGateRepository.FindWithSmartHome(id);
            _vehicleGateHandler.ChangeMode(vehicleGate, isPublic);

            string action = isPublic ? "mode_public" : "mode_private";

            var fields = new Dictionary<string, object>
            {
                { "action", action }

            };
            var tags = new Dictionary<string, string>
            {
                { "username", username},
                { "deviceId", id.ToString()}
            };
            SaveAction(fields, tags, id);

            vehicleGate.IsPublic = isPublic;
            await _vehicleGateRepository.Update(vehicleGate);
        }

      

        public async Task ToggleVehicleGate(Guid id, bool turnOn, string username)
        {
            VehicleGate vehicleGate = await _vehicleGateRepository.FindWithSmartHome(id);
            await _vehicleGateHandler.ToggleSmartDevice(vehicleGate, turnOn);

            string action = turnOn ? "turn_on_gate" : "turn_off_gate";

            var fields = new Dictionary<string, object>
            {
                { "action", action }

            };
            var tags = new Dictionary<string, string>
            {
                { "username", username},
                { "deviceId", id.ToString()}
            };

            SaveAction(fields, tags, id);

            vehicleGate.IsOn = turnOn;
            await _vehicleGateRepository.Update(vehicleGate);
        }


        public Task<VehicleGate> Delete(Guid id)
        {
            return _vehicleGateRepository.Delete(id);
        }

        public Task<VehicleGate> Get(Guid id)
        {
            return _vehicleGateRepository.Read(id);
        }

        public Task<IEnumerable<VehicleGate>> GetAll()
        {
            return _vehicleGateRepository.ReadAll();
        }

        public Task<VehicleGate> Update(VehicleGate entity)
        {
            return _vehicleGateRepository.Update(entity);
        }

        
    }
}
