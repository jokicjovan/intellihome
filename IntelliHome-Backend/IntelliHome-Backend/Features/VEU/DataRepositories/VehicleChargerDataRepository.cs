using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;

namespace IntelliHome_Backend.Features.VEU.DataRepositories
{
    public class VehicleChargerDataRepository: IVehicleChargerDataRepository
    {
        private readonly InfluxRepository _influxRepository;

        public VehicleChargerDataRepository(InfluxRepository influxRepository)
        {
            _influxRepository = influxRepository;
        }

        public void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("vehicleChargerAction", fields, tags);
        }

        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("vehicleChargerAction", id, from, to).Result;
            return result.Select(ConvertToActionDataDTO).ToList();
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
