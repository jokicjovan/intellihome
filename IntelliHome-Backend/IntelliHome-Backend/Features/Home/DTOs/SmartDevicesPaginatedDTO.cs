using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class SmartDevicesPaginatedDTO
    {
        public IEnumerable<SmartDeviceDTO> SmartDevices { get; set; }
        public Int32 TotalCount { get; set; }

        public SmartDevicesPaginatedDTO()
        {
        }
    }
}
