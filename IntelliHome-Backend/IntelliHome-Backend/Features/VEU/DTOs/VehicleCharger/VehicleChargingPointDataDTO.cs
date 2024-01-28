using System.ComponentModel.DataAnnotations.Schema;

namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargingPointDataDTO
    {
        public DateTime? Timestamp { get; set; }
        public Guid ChargingPointId { get; set; }
        public double Capacity { get; set; }
        public double ChargeLimit { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double CurrentCapacity { get; set; }
        public VehicleChargingPointStatus Status { get; set; }
        public double TotalConsumption { get; set; }
    }
}
