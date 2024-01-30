using System.ComponentModel.DataAnnotations.Schema;

namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargingPointDataDTO
    {
        public DateTime? Timestamp { get; set; }
        public Guid Id { get; set; }
        public Double CurrentCapacity { get; set; }
    }
}
