using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.VEU.DTOs.BatterySystem
{
    public class BatterySystemDTO : SmartDeviceDTO
    {
        public double Capacity { get; set; }
        public double CurrentCapacity { get; set; }

        public BatterySystemDTO()
        {
        }
    }
}
