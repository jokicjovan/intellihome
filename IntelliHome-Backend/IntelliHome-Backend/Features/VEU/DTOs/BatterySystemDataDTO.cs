namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class BatterySystemDataDTO
    {
        public DateTime? Timestamp { get; set; }
        public Double CurrentCapacity { get; set; }
        public Double ConsumptionPerMinute { get; set; }
        public Double GridPerMinute { get; set; }
    }
}
