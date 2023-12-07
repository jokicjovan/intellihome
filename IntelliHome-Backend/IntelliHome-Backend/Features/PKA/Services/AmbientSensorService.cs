using Data.Context;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SendGrid.Helpers.Mail;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AmbientSensorService : IAmbientSensorService
    {
        private readonly IAmbientSensorRepository _ambientSensorRepository;
        private readonly IAmbientSensorHandler _ambientSensorHandler;
        private readonly IAmbientSensorDataRepository _ambientSensorDataRepository;

        public AmbientSensorService(IAmbientSensorRepository ambientSensorRepository, IAmbientSensorHandler ambientSensorHandler, IAmbientSensorDataRepository ambientSensorDataRepository)
        {
            _ambientSensorRepository = ambientSensorRepository;
            _ambientSensorHandler = ambientSensorHandler;
            _ambientSensorDataRepository = ambientSensorDataRepository;
            
        }

        public async Task<AmbientSensor> Create(AmbientSensor entity)
        {
            entity = await _ambientSensorRepository.Create(entity);
            bool success = await _ambientSensorHandler.ConnectToSmartDevice(entity, new Dictionary<string, object>());
            if (success)
            {
                entity.IsConnected = true;
                await _ambientSensorRepository.Update(entity);
            }
            return entity;
        }


        public async Task<AmbientSensorDTO> GetById(Guid id)
        {
            AmbientSensor ambientSensor = await _ambientSensorRepository.Read(id);
            AmbientSensorDTO ambientSensorDTO = new AmbientSensorDTO
            {
                Id = ambientSensor.Id,
                Name = ambientSensor.Name,
                IsConnected = ambientSensor.IsConnected,
                IsOn = ambientSensor.IsOn,
                Category = ambientSensor.Category,
                Type = ambientSensor.Type,
                PowerPerHour = ambientSensor.PowerPerHour,
            };

            AmbientSensorData ambientSensorData = GetLastData(id);

            if (ambientSensorData != null)
            {
                ambientSensorDTO.Temperature = ambientSensorData.Temperature;
                ambientSensorDTO.Humidity = ambientSensorData.Humidity;
            }

            return ambientSensorDTO;
        }


        private AmbientSensorData GetLastData(Guid id)
        {
            return _ambientSensorDataRepository.GetLastData(id);
        }

        public List<AmbientSensorHistoricalDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _ambientSensorDataRepository.GetHistoricalData(id, from, to);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _ambientSensorDataRepository.AddPoint(fields, tags);
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
