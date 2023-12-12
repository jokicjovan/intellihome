using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class SolarPanelSystemDTO : SmartDeviceDTO
    {
        public Double ProductionPerMinute { get; set; }
        public Double Area { get; set; }
        public Double Efficiency { get; set; }
        public SolarPanelSystemDTO() { }
    }
}
