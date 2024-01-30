using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.Shared.Influx;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;

namespace IntelliHome_Backend.Features.VEU.DataRepositories
{
    public class VehicleChargingPointDataRepository: IVehicleChargingPointDataRepository
    {
        private readonly InfluxRepository _influxRepository;

        public VehicleChargingPointDataRepository(InfluxRepository influxRepository)
        {
            _influxRepository = influxRepository;
        }

        public void AddVehicleChargingPointMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _influxRepository.WriteToInfluxAsync("vehicleChargingPointData", fields, tags);
        }

        public List<VehicleChargingPointDataDTO> GetVehicleChargingPointHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _influxRepository.GetHistoricalData("vehicleChargingPointData", id, from, to).Result;
            return result.Select(ConvertToVehicleChargingPointDataDTO).ToList();
        }

        public VehicleChargingPointDataDTO GetLastVehicleChargingPointData(Guid id)
        {
            var table = _influxRepository.GetLastData("vehicleChargingPointData", id).Result;
            return table == null ? new VehicleChargingPointDataDTO() : ConvertToVehicleChargingPointDataDTO(table);
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
