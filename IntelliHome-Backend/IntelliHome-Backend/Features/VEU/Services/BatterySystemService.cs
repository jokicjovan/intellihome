using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class BatterySystemService : IBatterySystemService
    {
        private readonly IBatterySystemRepository _batterySystemRepository;
        private readonly IBatterySystemDataRepository _batterySystemDataRepository;
        private readonly IBatterySystemHandler _batterySystemHandler;

        public BatterySystemService(IBatterySystemRepository batterySystemRepository, IBatterySystemHandler batterySystemHandler,
            IBatterySystemDataRepository batterySystemDataRepository)
        {
            _batterySystemRepository = batterySystemRepository;
            _batterySystemHandler = batterySystemHandler;
            _batterySystemDataRepository = batterySystemDataRepository;
        }

        public async Task<BatterySystem> Create(BatterySystem entity)
        {
            entity = await _batterySystemRepository.Create(entity);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "capacity", entity.Capacity }
                        };
            bool success = await _batterySystemHandler.ConnectToSmartDevice(entity, additionalAttributes);
            if (success)
            {
                entity.IsConnected = true;
                await _batterySystemRepository.Update(entity);
            }
            return entity;
        }

        public Task<BatterySystem> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<BatterySystem> Get(Guid id)
        {
            BatterySystem batterySystem = await _batterySystemRepository.Read(id);
            if (batterySystem == null)
            {
                throw new ResourceNotFoundException("Battery system with provided Id not found!");
            }
            return batterySystem;
        }

        public Task<IEnumerable<BatterySystem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BatterySystem> Update(BatterySystem entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BatterySystemDTO> GetWithData(Guid id)
        {
            BatterySystem batterySystem = await _batterySystemRepository.Read(id);
            BatterySystemDTO batterySystemDTO = new BatterySystemDTO
            {
                Id = batterySystem.Id,
                Name = batterySystem.Name,
                IsConnected = batterySystem.IsConnected,
                IsOn = batterySystem.IsOn,
                Category = batterySystem.Category,
                Type = batterySystem.Type
            };

            BatterySystemDataDTO batterySystemDataDTO = _batterySystemDataRepository.GetLastData(id);
            if (batterySystemDataDTO != null)
            {
                batterySystemDTO.CurrentCapacity = batterySystemDataDTO.CurrentCapacity;
                batterySystemDTO.ConsumptionPerMinute = batterySystemDataDTO.ConsumptionPerMinute;
                batterySystemDTO.GridPerMinute = batterySystemDataDTO.GridPerMinute;
            }

            return batterySystemDTO;
        }

        public List<BatterySystemDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _batterySystemDataRepository.GetHistoricalData(id, from, to);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _batterySystemDataRepository.AddPoint(fields, tags);
        }
    }
}
