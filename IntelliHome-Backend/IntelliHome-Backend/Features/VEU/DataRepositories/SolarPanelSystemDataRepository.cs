using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem;

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
            _influxRepository.WriteToInfluxAsync("solarPanelSystemProduction", fields, tags);
        }

        public void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("solarPanelSystemAction", fields, tags);
        }

        public List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("solarPanelSystemProduction", id, from, to).Result;
            return result.Select(ConvertToSolarPanelSystemProductionDataDTO).ToList();
        }

        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("solarPanelSystemAction", id, from, to).Result;
            return result.Select(ConvertToActionDataDTO).ToList();
        }

        public SolarPanelSystemProductionDataDTO GetLastProductionData(Guid id)
        {
            var table = _influxRepository.GetLastData("solarPanelSystemProduction", id).Result;
            return table == null ?  new SolarPanelSystemProductionDataDTO() : ConvertToSolarPanelSystemProductionDataDTO(table);
        }

        private SolarPanelSystemProductionDataDTO ConvertToSolarPanelSystemProductionDataDTO(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var productionPerMinuteRecord = rows.FirstOrDefault(r => r.Row.Contains("productionPerMinute"));
            double productionPerMinute = productionPerMinuteRecord != null ? Convert.ToDouble(productionPerMinuteRecord.GetValueByKey("_value")) : 0.0;

            return new SolarPanelSystemProductionDataDTO
            {
                Timestamp = timestamp,
                ProductionPerMinute = productionPerMinute
            };
        }

        private ActionDataDTO ConvertToActionDataDTO(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var actionRecord = rows.FirstOrDefault(r => r.Row.Contains("action"));
            string action = actionRecord != null ? actionRecord.GetValueByKey("_value").ToString() : "";

            string actionBy = rows[0].GetValueByKey("actionBy") != null ? rows[0].GetValueByKey("actionBy").ToString() : "";

            return new ActionDataDTO
            {
                Timestamp = timestamp,
                Action = action,
                ActionBy = actionBy
            };
        }

    }
}
