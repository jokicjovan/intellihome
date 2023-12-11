using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Shared.Influx;

namespace IntelliHome_Backend.Features.Home.DataRepository
{
    public class SmartHomeDataRepository : ISmartHomeDataRepository
    {
        private readonly InfluxRepository _influxRepository;

        public SmartHomeDataRepository(InfluxRepository influxRepository)
        {
            _influxRepository = influxRepository;
        }
        public void AddUsageMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("smart_home_usage", fields, tags);
        }

        public List<SmartHomeUsageDataDTO> GetUsageHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("smart_home_usage", id, from, to).Result;
            return result.Select(ConvertToSmartHomeUsageDataDTO).ToList();
        }

        public SmartHomeUsageDataDTO GetLastUsageData(Guid id)
        {
            var table = _influxRepository.GetLastData("smart_home_usage", id).Result;
            return ConvertToSmartHomeUsageDataDTO(table);
        }

        private SmartHomeUsageDataDTO ConvertToSmartHomeUsageDataDTO(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var consumptionPerMinuteRecord = rows.FirstOrDefault(r => r.Row.Contains("production_per_minute"));
            double consumptionPerMinute = consumptionPerMinuteRecord != null ? Convert.ToDouble(consumptionPerMinuteRecord.GetValueByKey("_value")) : 0.0;

            var productionPerMinuteRecord = rows.FirstOrDefault(r => r.Row.Contains("production_per_minute"));
            double productionPerMinute = productionPerMinuteRecord != null ? Convert.ToDouble(productionPerMinuteRecord.GetValueByKey("_value")) : 0.0;

            var gridPerMinuteRecord = rows.FirstOrDefault(r => r.Row.Contains("production_per_minute"));
            double gridPerMinute = gridPerMinuteRecord != null ? Convert.ToDouble(gridPerMinuteRecord.GetValueByKey("_value")) : 0.0;

            return new SmartHomeUsageDataDTO
            {
                Timestamp = timestamp,
                ConsumptionPerMinute = consumptionPerMinute,
                ProductionPerMinute = productionPerMinute,
                GridPerMinute = gridPerMinute
            };
        }
    }
}
