using Data.Models.SPU;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Handlers;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class SolarPanelSystemService : ISolarPanelSystemService
    {
        private readonly ISolarPanelSystemRepository _solarPanelSystemRepository;
        private readonly ISolarPanelSystemDataRepository _solarPanelSystemDataRepository;
        private readonly ISolarPanelSystemHandler _solarPanelSystemHandler;

        public SolarPanelSystemService(ISolarPanelSystemRepository solarPanelSystemRepository, ISolarPanelSystemDataRepository solarPanelSystemDataRepository, ISolarPanelSystemHandler solarPanelSystemHandler)
        {
            _solarPanelSystemRepository = solarPanelSystemRepository;
            _solarPanelSystemDataRepository = solarPanelSystemDataRepository;
            _solarPanelSystemHandler = solarPanelSystemHandler;
        }

        public async Task<SolarPanelSystem> Create(SolarPanelSystem entity)
        {
            entity = await _solarPanelSystemRepository.Create(entity);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
            {
                { "area", entity.Area },
                { "efficiency", entity.Efficiency }
            };
            bool success = await _solarPanelSystemHandler.ConnectToSmartDevice(entity, additionalAttributes);
            if (success)
            {
                entity.IsConnected = true;
                await _solarPanelSystemRepository.Update(entity);
            }
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
            SolarPanelSystem solarPanelSystem = await _solarPanelSystemRepository.Read(id);
            SolarPanelSystemDTO solarPanelSystemDTO = new SolarPanelSystemDTO
            {
                Id = solarPanelSystem.Id,
                Name = solarPanelSystem.Name,
                IsConnected = solarPanelSystem.IsConnected,
                IsOn = solarPanelSystem.IsOn,
                Category = solarPanelSystem.Category.ToString(),
                Type = solarPanelSystem.Type.ToString(),
                Area = solarPanelSystem.Area,
                Efficiency = solarPanelSystem.Efficiency
            };

            SolarPanelSystemProductionDataDTO solarPanelSystemDataDTO = _solarPanelSystemDataRepository.GetLastProductionData(id);
            solarPanelSystemDTO.ProductionPerMinute = solarPanelSystemDataDTO.ProductionPerMinute;

            return solarPanelSystemDTO;
        }

        public List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _solarPanelSystemDataRepository.GetProductionHistoricalData(id, from, to);
        }

        public void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _solarPanelSystemDataRepository.AddProductionMeasurement(fields, tags);
        }

        public async Task ToggleSolarPanelSystem(Guid id, bool turnOn = true) {
            SolarPanelSystem solarPanelSystem = await _solarPanelSystemRepository.FindWithSmartHome(id);
            if (solarPanelSystem == null)
            {
                throw new ResourceNotFoundException("Smart device not found!");
            }
            await _solarPanelSystemHandler.ToggleSmartDevice(solarPanelSystem, turnOn);
            solarPanelSystem.IsOn = turnOn;
            _ = _solarPanelSystemRepository.Update(solarPanelSystem);
        }
    }
}
