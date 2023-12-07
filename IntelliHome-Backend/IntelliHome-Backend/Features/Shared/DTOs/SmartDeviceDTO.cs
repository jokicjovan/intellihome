using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Shared.DTOs
{
    public class SmartDeviceDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public SmartDeviceCategory Category { get; set; }
        public SmartDeviceType Type { get; set; }
        public Boolean IsConnected { get; set; }
        public Boolean IsOn { get; set; }
    }
}
