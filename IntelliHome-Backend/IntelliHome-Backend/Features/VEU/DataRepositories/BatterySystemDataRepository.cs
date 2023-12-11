using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories
{
    public class BatterySystemDataRepository : IBatterySystemDataRepository
    {
        private readonly InfluxRepository _influxRepository;

        public BatterySystemDataRepository(InfluxRepository influxRepository)
        {
            _influxRepository = influxRepository;
        }
        public void AddCapacityMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("battery_system_capacity", fields, tags);
        }

        public List<BatterySystemCapacityDataDTO> GetCapacityHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("battery_system_capacity", id, from, to).Result;
            return result.Select(ConvertToBatterySystemCapacityDataDTO).ToList();
        }

        public BatterySystemCapacityDataDTO GetLastCapacityData(Guid id)
        {
            var table = _influxRepository.GetLastData("battery_system_capacity", id).Result;
            return ConvertToBatterySystemCapacityDataDTO(table);
        }

        private BatterySystemCapacityDataDTO ConvertToBatterySystemCapacityDataDTO(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var currentCapacityRecord = rows.FirstOrDefault(r => r.Row.Contains("current_capacity"));
            double currentCapacity = currentCapacityRecord != null ? Convert.ToDouble(currentCapacityRecord.GetValueByKey("_value")) : 0.0;

            return new BatterySystemCapacityDataDTO {
                Timestamp = timestamp,
                CurrentCapacity = currentCapacity
            };
        }
    }
}
