namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class SprinklerData
    {
        public Boolean IsSpraying { get; set; }
        public Double ConsumptionPerMinute { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
