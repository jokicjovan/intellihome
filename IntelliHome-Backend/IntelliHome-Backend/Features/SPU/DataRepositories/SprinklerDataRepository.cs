using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.DataRepositories
{
    public class SprinklerDataRepository : ISprinklerDataRepository
    {
        private readonly InfluxRepository _context;

        public SprinklerDataRepository(InfluxRepository context)
        {
            _context = context;
        }

        public List<SprinklerData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData("sprinkler", id, from, to).Result;
            return result.Select(ConvertToSprinklerData).ToList();
        }

        public List<SprinklerData> GetLastHourData(Guid id)
        {
            var result = _context.GetLastHourData(id).Result;
            return result.Select(ConvertToSprinklerData).ToList();
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("sprinkler", fields, tags);
        }

        public SprinklerData GetLastData(Guid id)
        {
            var table = _context.GetLastData("sprinkler", id).Result;
            return table == null || table.Records.Count == 0 ? new SprinklerData() : ConvertToSprinklerData(table);
        }

        private SprinklerData ConvertToSprinklerData(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var isSprayingRecord = rows.FirstOrDefault(r => r.Row.Contains("isSpraying"));

            bool isSpraying = isSprayingRecord != null && Convert.ToBoolean(isSprayingRecord.GetValueByKey("_value"));

            return new SprinklerData
            {
                Timestamp = timestamp,
                IsSpraying = isSpraying
            };
        }
    }
}
