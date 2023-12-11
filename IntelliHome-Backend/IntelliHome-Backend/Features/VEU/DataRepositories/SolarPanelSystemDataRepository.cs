using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories
{
    public class SolarPanelSystemDataRepository : ISolarPanelSystemDataRepository
    {
        private readonly InfluxRepository _influxRepository;

        public SolarPanelSystemDataRepository(InfluxRepository influxRepository)
        {
            _influxRepository = influxRepository;
        }
        public void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("solar_panel_system_production", fields, tags);
        }

        public List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("solar_panel_system_production", id, from, to).Result;
            return result.Select(ConvertToSolarPanelSystemProductionDataDTO).ToList();
        }

        public SolarPanelSystemProductionDataDTO GetLastProductionData(Guid id)
        {
            var table = _influxRepository.GetLastData("solar_panel_system_production", id).Result;
            return ConvertToSolarPanelSystemProductionDataDTO(table);
        }

        private SolarPanelSystemProductionDataDTO ConvertToSolarPanelSystemProductionDataDTO(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var productionPerMinuteRecord = rows.FirstOrDefault(r => r.Row.Contains("production_per_minute"));
            double productionPerMinute = productionPerMinuteRecord != null ? Convert.ToDouble(productionPerMinuteRecord.GetValueByKey("_value")) : 0.0;

            return new SolarPanelSystemProductionDataDTO
            {
                Timestamp = timestamp,
                ProductionPerMinute = productionPerMinute
            };
        }
    }
}
