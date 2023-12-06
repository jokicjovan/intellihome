using Data.Context;
using Data.Models.PKA;
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
        private readonly InfluxDbContext _influxDbContext;

        public AmbientSensorService(IAmbientSensorRepository ambientSensorRepository, IAmbientSensorHandler ambientSensorHandler, InfluxDbContext influxDbContext)
        {
            _ambientSensorRepository = ambientSensorRepository;
            _ambientSensorHandler = ambientSensorHandler;
            _influxDbContext = influxDbContext;
        }

        public async Task<AmbientSensor> Create(AmbientSensor entity)
        {
            entity = await _ambientSensorRepository.Create(entity);
            bool success = await _ambientSensorHandler.AddSmartDeviceToSimulator(entity, new Dictionary<string, object>());
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


        public List<AmbientSensorHistoricalDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            //TODO: timezones
            var query = $"from(bucket: \"intellihome_influx\") " +
                        $"|> range(start: {from:yyyy-MM-ddTHH:mm:ssZ}, stop: {to:yyyy-MM-ddTHH:mm:ssZ}) " +
                        $"|> filter(fn: (r) => r._measurement == \"ambient_sensor\" and r.deviceId == \"{id}\") " +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"deviceId\"])";


            var result = _influxDbContext.QueryFromInfluxAsync(query).Result;

            List<AmbientSensorHistoricalDataDTO> list = new List<AmbientSensorHistoricalDataDTO>();

            foreach (var table in result)
            {
                var rows = table.Records;
                DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
                TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);
                double temperature = 0;
                double humidity = 0;
                if (rows[0].Row.Contains("temperature"))
                {
                    temperature = Convert.ToDouble(rows[0].GetValueByKey("_value"));
                    humidity = Convert.ToDouble(rows[1].GetValueByKey("_value"));
                }
                else
                {
                    temperature = Convert.ToDouble(rows[1].GetValueByKey("_value"));
                    humidity = Convert.ToDouble(rows[0].GetValueByKey("_value"));
                }

                AmbientSensorHistoricalDataDTO dto = new AmbientSensorHistoricalDataDTO
                {
                    Timestamp = timestamp,
                    Temperature = temperature,
                    Humidity = humidity
                };

                list.Add(dto);
            }

            return list;
        }
    }
}
