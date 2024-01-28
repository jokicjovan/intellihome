namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargerDataDTO
    {
        public DateTime? Timestamp { get; set; }
        public double ConsumptionPerMinute { get; set; }
        public List<VehicleChargingPointDataDTO> BusyChargingPoints { get; set; }
    }
}
