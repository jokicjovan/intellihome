using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories
{
    public class SolarPanelSystemDataRepository : ISolarPanelSystemDataRepository
    {
        private readonly InfluxRepository _context;

        public SolarPanelSystemDataRepository(InfluxRepository context)
        {
            _context = context;
        }

        public List<SolarPanelSystemDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData(id, from, to).Result;
            return result.Select(ConvertToSolarPanelSystemDataDTO).ToList();
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("battery_system", fields, tags);
        }

        public SolarPanelSystemDataDTO GetLastData(Guid id)
        {
            var table = _context.GetLastData(id).Result;
            return ConvertToSolarPanelSystemDataDTO(table);
        }

        private SolarPanelSystemDataDTO ConvertToSolarPanelSystemDataDTO(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var productionPerMinuteRecord = rows.FirstOrDefault(r => r.Row.Contains("production_per_minute"));
            double productionPerMinute = productionPerMinuteRecord != null ? Convert.ToDouble(productionPerMinuteRecord.GetValueByKey("_value")) : 0.0;

            return new SolarPanelSystemDataDTO
            {
                Timestamp = timestamp,
                ProductionPerMinute = productionPerMinute
            };
        }
    }
}
