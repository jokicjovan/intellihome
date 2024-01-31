using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem
{
    public class SolarPanelSystemDTO : SmartDeviceDTO
    {
        public double ProductionPerMinute { get; set; }
        public double Area { get; set; }
        public double Efficiency { get; set; }
        public SolarPanelSystemDTO() { }
    }
}
