using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Influx;

namespace IntelliHome_Backend.Features.PKA.DataRepositories
{
    public class AirConditionerDataRepository: IAirConditionerDataRepository
    {
        private readonly InfluxRepository _context;

        public AirConditionerDataRepository(InfluxRepository context)
        {
            _context = context;
        }

        public List<AirConditionerData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData("airConditioner", id, from, to).Result;
            return result.Select(ConvertToAirConditionerData).ToList();
        }

        public List<AirConditionerData> GetLastHourData(Guid id)
        {
            var result = _context.GetLastHourData(id).Result;
            return result.Select(ConvertToAirConditionerData).ToList();
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("airConditioner", fields, tags);
        }
        public void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("airConditionerAction", fields, tags);
        }

        public AirConditionerData GetLastData(Guid id)
        {
            var table = _context.GetLastData("airConditioner", id).Result;
            return table == null || table.Records.Count == 0 ? new AirConditionerData() : ConvertToAirConditionerData(table);
        }
        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData("airConditionerAction", id, from, to).Result;
            return result.Select(ConvertToActionDataDTO).ToList();
        }

        private AirConditionerData ConvertToAirConditionerData(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var temperatureRecord = rows.FirstOrDefault(r => r.Row.Contains("temperature"));
            var modeRecord = rows.FirstOrDefault(r => r.Row.Contains("mode"));

            double temperature = temperatureRecord != null ? Convert.ToDouble(temperatureRecord.GetValueByKey("_value")) : 0.0;
            string mode = rows[0].GetValueByKey("mode") != null ? rows[0].GetValueByKey("mode").ToString() : "";

            return new AirConditionerData
            {
                Temperature = temperature,
                Mode=mode,
                Timestamp = timestamp,
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
