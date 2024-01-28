namespace IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem
{
    public class SolarPanelSystemProductionDataDTO
    {
        public DateTime? Timestamp { get; set; }
        public double ProductionPerMinute { get; set; }
    }
}
