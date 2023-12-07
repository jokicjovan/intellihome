using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class LampService : ILampService
    {
        private readonly ILampRepository _lampRepository;
        private readonly ILampHandler _lampHandler;

        public LampService(ILampRepository lampRepository, ILampHandler lampHandler)
        {
            _lampRepository = lampRepository;
            _lampHandler = lampHandler;
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
