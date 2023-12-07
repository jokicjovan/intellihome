using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.Repositories;

namespace IntelliHome_Backend.Features.PKA.DataRepositories
{
    public class AmbientSensorDataRepository : IAmbientSensorDataRepository
    {
        private readonly InfluxRepository _context;

        public AmbientSensorDataRepository(InfluxRepository context)
        {
            _context = context;
        }

        public List<AmbientSensorData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData(id, from, to).Result;
            return result.Select(ConvertToAmbientSensorData).ToList();
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("ambient_sensor", fields, tags);
        }

        public AmbientSensorData GetLastData(Guid id)
        {
           
            var table = _context.GetLastData(id).Result;

            AmbientSensorData data = ConvertToAmbientSensorData(table);

            return new AmbientSensorData
            {
                Temperature = data.Temperature,
                Humidity = data.Humidity,
            };
        }


        private AmbientSensorData ConvertToAmbientSensorData(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var temperatureRecord = rows.FirstOrDefault(r => r.Row.Contains("temperature"));
            var humidityRecord = rows.FirstOrDefault(r => r.Row.Contains("humidity"));

            double temperature = temperatureRecord != null ? Convert.ToDouble(temperatureRecord.GetValueByKey("_value")) : 0.0;
            double humidity = humidityRecord != null ? Convert.ToDouble(humidityRecord.GetValueByKey("_value")) : 0.0;

            return new AmbientSensorData
            {
                Timestamp = timestamp,
                Temperature = temperature,
                Humidity = humidity
            };
        }
    }

}
