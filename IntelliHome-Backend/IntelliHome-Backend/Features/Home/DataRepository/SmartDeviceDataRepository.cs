using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Shared.Influx;

namespace IntelliHome_Backend.Features.Home.DataRepository
{
    public class SmartDeviceDataRepository : ISmartDeviceDataRepository
    {
        private readonly InfluxRepository _context;

        public SmartDeviceDataRepository(InfluxRepository context)
        {
            _context = context;
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("availability", fields, tags);
        }


        public List<AvailabilityData> GetAvailabilityData(Guid id, string h)
        {
            var result = _context.GetHistoricalAvailability(id, h).Result;
            return result.Select(ConvertToAvailability).ToList();   
        }

        public AvailabilityData ConvertToAvailability(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            float duration = float.Parse(rows[0].GetValueByKey("duration").ToString());
            float percentage = float.Parse(rows[0].GetValueByKey("percentage").ToString());

            return new AvailabilityData
            {
                Timestamp = timestamp,
                Duration = duration,
                Percentage = percentage
            };
        }

    }
}
