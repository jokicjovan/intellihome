using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class SolarPanelSystemDTO : SmartDeviceDTO
    {
        public Double CreatedPower { get; set; }
        public SolarPanelSystemDTO() { }
    }
}
