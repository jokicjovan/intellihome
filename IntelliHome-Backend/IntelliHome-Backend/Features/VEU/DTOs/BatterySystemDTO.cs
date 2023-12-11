using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class BatterySystemDTO : SmartDeviceDTO
    {
        public Double CurrentCapacity { get; set; }
        public Double ConsumptionPerMinute { get; set; }
        public Double GridPerMinute { get; set; }

        public BatterySystemDTO()
        {
        }
    }
}
