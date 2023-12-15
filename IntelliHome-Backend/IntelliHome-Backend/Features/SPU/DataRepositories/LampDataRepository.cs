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
            var table = _influxRepository.GetLastData("lamp", id).Result;
            return table == null || table.Records.Count == 0  ? new LampData() : ConvertToLampData(table);
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("lamp", fields, tags);
        }

        public List<LampData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("lamp", id, from, to).Result;
            return result.Select(ConvertToLampData).ToList();
        }

        public LampData ConvertToLampData(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var currentBrightnessRecord = rows.FirstOrDefault(r => r.Row.Contains("currentBrightness"));
            var isShiningRecord = rows.FirstOrDefault(r => r.Row.Contains("isShining"));
            var isAutoRecord = rows.FirstOrDefault(r => r.Row.Contains("isAuto"));

            double currentBrightness = currentBrightnessRecord != null ? Convert.ToDouble(currentBrightnessRecord.GetValueByKey("_value")) : 0.0;
            bool isShining = isShiningRecord != null && Convert.ToBoolean(isShiningRecord.GetValueByKey("_value"));
            bool isAuto = isAutoRecord != null && Convert.ToBoolean(isAutoRecord.GetValueByKey("_value"));

            return new LampData
            {
                Timestamp = timestamp,
                CurrentBrightness = currentBrightness,
                IsShining = isShining,
                IsAuto = isAuto
            };
        }
    }
}
