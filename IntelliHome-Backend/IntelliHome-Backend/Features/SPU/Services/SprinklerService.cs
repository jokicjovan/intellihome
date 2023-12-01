using Data.Models.SPU;
using IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class SprinklerService : ISprinklerService
    {
        private readonly ISprinklerRepository _sprinklerRepository;

        public SprinklerService(ISprinklerRepository sprinklerRepository)
        {
            _sprinklerRepository = sprinklerRepository;
        }

        public async Task<Sprinkler> Create(Sprinkler entity)
        {
            entity = await _sprinklerRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _sprinklerRepository.Update(entity);
            //}
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
