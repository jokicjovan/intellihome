using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class SolarPanelSystemService : ISolarPanelSystemService
    {
        private readonly ISolarPanelSystemRepository _solarPanelSystemRepository;
        private readonly ISolarPanelSystemDataRepository _solarPanelSystemDataRepository;
        private readonly ISolarPanelSystemHandler _solarPanelSystemHandler;
        private readonly ISmartDeviceDataRepository _smartDeviceDataRepository;

        public SolarPanelSystemService(
            ISolarPanelSystemRepository solarPanelSystemRepository, 
            ISolarPanelSystemDataRepository solarPanelSystemDataRepository,
            ISolarPanelSystemHandler solarPanelSystemHandler,
            ISmartDeviceDataRepository smartDeviceDataRepository)
        {
            _solarPanelSystemRepository = solarPanelSystemRepository;
            _solarPanelSystemDataRepository = solarPanelSystemDataRepository;
            _solarPanelSystemHandler = solarPanelSystemHandler;
            _smartDeviceDataRepository = smartDeviceDataRepository;
        }

        public async Task<SolarPanelSystem> Create(SolarPanelSystem entity)
        {
            entity = await _solarPanelSystemRepository.Create(entity);
            bool success = await _solarPanelSystemHandler.ConnectToSmartDevice(entity);
            if (!success) return entity;
            entity.IsConnected = true;
            await _solarPanelSystemRepository.Update(entity);
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

        public Task<SolarPanelSystem> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<SolarPanelSystem> Get(Guid id)
        {
            SolarPanelSystem solarPanelSystem = await _solarPanelSystemRepository.Read(id);
            if (solarPanelSystem == null)
            {
                throw new ResourceNotFoundException("Solar panel system with provided Id not found!");
            }
            return solarPanelSystem;
        }

        public async Task<SolarPanelSystem> GetWithHome(Guid id)
        {
            SolarPanelSystem solarPanelSystem = await _solarPanelSystemRepository.FindWithSmartHome(id);
            if (solarPanelSystem == null)
            {
                throw new ResourceNotFoundException("Solar panel system with provided Id not found!");
            }
            return solarPanelSystem;
        }

        public Task<IEnumerable<SolarPanelSystem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SolarPanelSystem> Update(SolarPanelSystem entity)
        {
            throw new NotImplementedException();
        }

        public async Task<SolarPanelSystemDTO> GetWithProductionData(Guid id)
        {
            SolarPanelSystem solarPanelSystem = await GetWithHome(id);
            SolarPanelSystemDTO solarPanelSystemDTO = new SolarPanelSystemDTO
            {
                Id = solarPanelSystem.Id,
                Name = solarPanelSystem.Name,
                IsConnected = solarPanelSystem.IsConnected,
                IsOn = solarPanelSystem.IsOn,
                Category = solarPanelSystem.Category.ToString(),
                Type = solarPanelSystem.Type.ToString(),
                SmartHomeId = solarPanelSystem.SmartHome.Id,
                Area = solarPanelSystem.Area,
                Efficiency = solarPanelSystem.Efficiency
            };

            SolarPanelSystemProductionDataDTO solarPanelSystemDataDTO = _solarPanelSystemDataRepository.GetLastProductionData(id);
            solarPanelSystemDTO.ProductionPerMinute = solarPanelSystemDataDTO.ProductionPerMinute;

            return solarPanelSystemDTO;
        }

        public void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _solarPanelSystemDataRepository.AddProductionMeasurement(fields, tags);
        }

        public List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _solarPanelSystemDataRepository.GetProductionHistoricalData(id, from, to);
        }

        public void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _solarPanelSystemDataRepository.AddActionMeasurement(fields, tags);
        }

        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _solarPanelSystemDataRepository.GetActionHistoricalData(id, from, to);
        }

        public async Task Toggle(Guid id, String togglerUsername, bool turnOn = true)
        {
            SolarPanelSystem solarPanelSystem = await GetWithHome(id);
            _ = _solarPanelSystemHandler.ToggleSmartDevice(solarPanelSystem, turnOn);
            solarPanelSystem.IsOn = turnOn;
            _ = _solarPanelSystemRepository.Update(solarPanelSystem);

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
    }
}
