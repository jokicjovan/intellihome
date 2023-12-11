using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class SprinklerService : ISprinklerService
    {
        private readonly ISprinklerRepository _sprinklerRepository;
        private readonly ISprinklerHandler _sprinklerHandler;
        public SprinklerService(ISprinklerRepository sprinklerRepository, ISprinklerHandler sprinklerHandler)
        {
            _sprinklerRepository = sprinklerRepository;
            _sprinklerHandler = sprinklerHandler;
        }

        public async Task<Sprinkler> Create(Sprinkler entity)
        {
            entity = await _sprinklerRepository.Create(entity);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "power_per_hour", entity.PowerPerHour},
                        };
            bool success = await _sprinklerHandler.ConnectToSmartDevice(entity, additionalAttributes);
            if (success)
            {
                entity.IsConnected = true;
                await _sprinklerRepository.Update(entity);
            }
            return entity;
        }

        public Task<Sprinkler> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Sprinkler> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sprinkler>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Sprinkler> Update(Sprinkler entity)
        {
            throw new NotImplementedException();
        }
    }
}
