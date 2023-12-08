using Data.Context;
using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;
using Microsoft.VisualBasic;

namespace IntelliHome_Backend.Features.SPU.DataRepositories
{
    public class LampDataRepository : ILampDataRepository
    {
        private readonly InfluxRepository _influxRepository;

        public LampDataRepository(InfluxRepository influxRepository)
        {
            _influxRepository = influxRepository;
        }

        public LampData GetLastData(Guid id)
        {
            var table = _influxRepository.GetLastData(id).Result;

            LampData lampData = new LampData();

            return new LampData
            {
                CurrentBrightness = lampData.CurrentBrightness,
            };
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("lamp", fields, tags);
        }

        public List<LampData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData(id, from, to).Result;
            return result.Select(ConvertToLampData).ToList();
        }

        public LampData ConvertToLampData(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var currentBrightnessRecord = rows.FirstOrDefault(r => r.Row.Contains("current_brightness"));

            double currentBrightness = currentBrightnessRecord != null ? Convert.ToDouble(currentBrightnessRecord.GetValueByKey("_value")) : 0.0;

            return new LampData
            {
                Timestamp = timestamp,
                CurrentBrightness = currentBrightness,
            };
        }
    }
}
