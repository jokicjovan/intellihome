using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class LampService : ILampService
    {
        private readonly ILampRepository _lampRepository;

        public LampService(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public async Task<Lamp> Create(Lamp entity)
        {
            entity = await _lampRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _lampRepository.Update(entity);
            //}
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
