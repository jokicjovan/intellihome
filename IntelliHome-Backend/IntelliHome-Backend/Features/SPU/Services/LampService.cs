using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class LampService : ILampService
    {
        private readonly ILampRepository _lampRepository;
        private readonly ILampDataRepository _lampDataRepository;
        private readonly ILampHandler _lampHandler;

        public LampService(ILampRepository lampRepository, ILampHandler lampHandler, ILampDataRepository _lampDataRepository)
        {
            _lampRepository = lampRepository;
            _lampHandler = lampHandler;
            _lampDataRepository = _lampDataRepository;
        }

        public async Task<Lamp> Create(Lamp entity)
        {
            entity = await _lampRepository.Create(entity);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
            {
                { "brightness_limit", entity.BrightnessLimit },
            };
            bool success = await _lampHandler.ConnectToSmartDevice(entity, additionalAttributes);
            if (success)
            {
                entity.IsConnected = true;
                await _lampRepository.Update(entity);
            }
            return entity;
        }


        public async Task<LampDTO> GetWithData(Guid id)
        {
            Lamp lamp = await _lampRepository.Read(id);
            LampDTO lampDTO = new LampDTO
            {
                Id = lamp.Id,
                Name = lamp.Name,
                IsConnected = lamp.IsConnected,
                IsOn = lamp.IsOn,
                Category = lamp.Category,
                Type = lamp.Type,
                PowerPerHour = lamp.PowerPerHour,
                BrightnessLimit = lamp.BrightnessLimit,
            };

            LampData lampData = GetLastData(id);

            if (lampData != null)
            {
                lampDTO.CurrentBrightness = lampData.CurrentBrightness;
            }

            return lampDTO;
        }

        private LampData GetLastData(Guid id)
        {
            return _lampDataRepository.GetLastData(id);
        }

        public List<LampData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _lampDataRepository.GetHistoricalData(id, from, to);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _lampDataRepository.AddPoint(fields, tags);
        }


        public Task<Lamp> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Lamp> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Lamp>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Lamp> Update(Lamp entity)
        {
            throw new NotImplementedException();
        }
    }
}
