using Data.Models.PKA;
using Data.Models.VEU;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;

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

        public async Task ToggleAmbientSensor(Guid id, bool turnOn = true)
        {
            AmbientSensor batterySystem = await _ambientSensorRepository.FindWithSmartHome(id);
            if (batterySystem == null)
            {
                throw new ResourceNotFoundException("Smart device not found!");
            }
            await _ambientSensorHandler.ToggleSmartDevice(batterySystem, turnOn);
            batterySystem.IsOn = turnOn;
            _ = _ambientSensorRepository.Update(batterySystem);
        }

        public async Task<AmbientSensorDTO> GetWithData(Guid id)
        {
            AmbientSensor ambientSensor = await _ambientSensorRepository.FindWithSmartHome(id);
            AmbientSensorDTO ambientSensorDTO = new AmbientSensorDTO
            {
                Id = ambientSensor.Id,
                Name = ambientSensor.Name,
                IsConnected = ambientSensor.IsConnected,
                IsOn = ambientSensor.IsOn,
                Category = ambientSensor.Category.ToString(),
                Type = ambientSensor.Type.ToString(),
                SmartHomeId = ambientSensor.SmartHome.Id,
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

        #region DataHistory

        private AmbientSensorData GetLastData(Guid id)
        {
            return _ambientSensorDataRepository.GetLastData(id);
        }

        public List<AmbientSensorData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _ambientSensorDataRepository.GetHistoricalData(id, from, to);
        }

        public List<AmbientSensorData> GetLastHourData(Guid id)
        {
            return _ambientSensorDataRepository.GetLastHourData(id);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _ambientSensorDataRepository.AddPoint(fields, tags);
        }

        #endregion

        #region CRUD
        public async Task<AmbientSensor> Create(AmbientSensor entity)
        {
            entity = await _ambientSensorRepository.Create(entity);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
            {
                { "power_per_hour", entity.PowerPerHour},
            };
            bool success = await _ambientSensorHandler.ConnectToSmartDevice(entity, additionalAttributes);
            if (success)
            {
                entity.IsConnected = true;
                await _ambientSensorRepository.Update(entity);
            }
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
        #endregion
    }
}
