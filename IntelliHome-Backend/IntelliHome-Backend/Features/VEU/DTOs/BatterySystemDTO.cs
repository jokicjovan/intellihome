using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class BatterySystemDTO : SmartDeviceDTO
    {
        public Double Capacity { get; set; }
        public Double CurrentCapacity { get; set; }

        public BatterySystemDTO()
        {
        }
    }
}
