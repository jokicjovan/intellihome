using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AirConditionerService : IAirConditionerService
    {
        private readonly IAirConditionerRepository _airConditionerRepository;

        public AirConditionerService(IAirConditionerRepository airConditionerRepository)
        {
            _airConditionerRepository = airConditionerRepository;
        }

        public async Task<AirConditioner> Create(AirConditioner entity)
        {
            entity = await _airConditionerRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _airConditionerRepository.Update(entity);
            //}
            return entity;
        }

        public Task<AirConditioner> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AirConditioner> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AirConditioner>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<AirConditioner> Update(AirConditioner entity)
        {
            throw new NotImplementedException();
        }
    }
}
