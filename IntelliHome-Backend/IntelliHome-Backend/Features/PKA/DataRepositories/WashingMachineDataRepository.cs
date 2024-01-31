using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Influx;

namespace IntelliHome_Backend.Features.PKA.DataRepositories
{
    public class WashingMachineDataRepository: IWashingMachineDataRepository
    {
        private readonly InfluxRepository _context;

        public WashingMachineDataRepository(InfluxRepository context)
        {
            _context = context;
        }

        public List<WashingMachineData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData("washingMachine", id, from, to).Result;
            return result.Select(ConvertToWashingMachineData).ToList();
        }

        public List<WashingMachineData> GetLastHourData(Guid id)
        {
            var result = _context.GetLastHourData(id).Result;
            return result.Select(ConvertToWashingMachineData).ToList();
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("washingMachine", fields, tags);
        }
        public void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("washingMachineAction", fields, tags);
        }

        public WashingMachineData GetLastData(Guid id)
        {
            var table = _context.GetLastData("washingMachine", id).Result;
            return table == null || table.Records.Count == 0 ? new WashingMachineData() : ConvertToWashingMachineData(table);
        }
        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData("washingMachineAction", id, from, to).Result;
            return result.Select(ConvertToActionDataDTO).ToList();
        }

        private WashingMachineData ConvertToWashingMachineData(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var temperatureRecord = rows.FirstOrDefault(r => r.Row.Contains("temperature"));
            var modeRecord = rows.FirstOrDefault(r => r.Row.Contains("mode"));

            double temperature = temperatureRecord != null ? Convert.ToDouble(temperatureRecord.GetValueByKey("_value")) : 0.0;
            string mode = rows[0].GetValueByKey("mode") != null ? rows[0].GetValueByKey("mode").ToString() : "";

            return new WashingMachineData
            {
                Temperature = temperature,
                Mode = mode,
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
