using Data.Context;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.PKA.DataRepositories
{
    public class AmbientSensorDataRepository : IAmbientSensorDataRepository
    {
        private readonly InfluxDbContext _context;

        public AmbientSensorDataRepository(InfluxDbContext context)
        {
            _context = context;
        }

        public List<AmbientSensorHistoricalDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var query = $"from(bucket: \"intellihome_influx\") " +
                        $"|> range(start: {from:yyyy-MM-ddTHH:mm:ssZ}, stop: {to:yyyy-MM-ddTHH:mm:ssZ}) " +
                        $"|> filter(fn: (r) => r._measurement == \"ambient_sensor\" and r.deviceId == \"{id}\") " +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"deviceId\"])";


            var result = _context.QueryFromInfluxAsync(query).Result;

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


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("ambient_sensor", fields, tags);
        }
    }

}
