namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class SmartHomeUsageDataDTO
    {
        public DateTime? Timestamp { get; set; }
        public Double ProductionPerMinute { get; set; }
        public Double ConsumptionPerMinute { get; set; }
        public Double GridPerMinute { get; set; }
    }
}
