using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;

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

        public void AddVehicleChargingPointMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("vehicleChargingPoint", fields, tags);
        }

        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("vehicleChargerAction", id, from, to).Result;
            return result.Select(ConvertToActionDataDTO).ToList();
        }

        public List<VehicleChargingPointDataDTO> GetVehicleChargingPointHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("vehicleChargingPoint", id, from, to).Result;
            return result.Select(ConvertToVehicleChargingPointDataDTO).ToList();
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

        private VehicleChargingPointDataDTO ConvertToVehicleChargingPointDataDTO(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);

            var currentCapacityRecord = rows.FirstOrDefault(r => r.Row.Contains("currentCapacity"));
            double currentCapacity = currentCapacityRecord != null ? Convert.ToDouble(currentCapacityRecord.GetValueByKey("_value")) : 0.0;

            return new VehicleChargingPointDataDTO
            {
                Timestamp = timestamp,
                CurrentCapacity = currentCapacity

            };
        }
    }
}
