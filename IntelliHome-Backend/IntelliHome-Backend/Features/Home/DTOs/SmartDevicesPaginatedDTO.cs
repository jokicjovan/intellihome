using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class SmartDevicesPaginatedDTO
    {
        public IEnumerable<SmartDevice> SmartDevices { get; set; }
        public Int32 TotalCount { get; set; }

        public SmartDevicesPaginatedDTO()
        {
        }
    }
}
