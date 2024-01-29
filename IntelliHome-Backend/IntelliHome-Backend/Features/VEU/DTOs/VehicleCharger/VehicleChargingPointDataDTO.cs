using System.ComponentModel.DataAnnotations.Schema;

namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargingPointDataDTO
    {
        public DateTime? Timestamp { get; set; }
        public Guid ChargingPointId { get; set; }
        public double CurrentCapacity { get; set; }
        public String Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
