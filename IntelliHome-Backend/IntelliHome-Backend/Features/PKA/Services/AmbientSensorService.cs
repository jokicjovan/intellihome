using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AmbientSensorService : IAmbientSensorService
    {
        private readonly IAmbientSensorRepository _ambientSensorRepository;

        public AmbientSensorService(IAmbientSensorRepository ambientSensorRepository)
        {
            _ambientSensorRepository = ambientSensorRepository;
        }

        public async Task<AmbientSensor> Create(AmbientSensor entity)
        {
            entity = await _ambientSensorRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _ambientSensorRepository.Update(entity);
            //}
            return entity;
        }

        public Task<AmbientSensor> Delete(Guid id)
        {
            return _ambientSensorRepository.Delete(id);
        }

        public Task<AmbientSensor> Get(Guid id)
        {
            return _ambientSensorRepository.Read(id);
        }

        public Task<IEnumerable<AmbientSensor>> GetAll()
        {
            return _ambientSensorRepository.ReadAll();
        }

        public IEnumerable<AmbientSensor> GetAllWithHome()
        {
            return _ambientSensorRepository.FindAllWIthHome();
        }

        public Task<AmbientSensor> Update(AmbientSensor entity)
        {
            return _ambientSensorRepository.Update(entity);
        }
    }
}
