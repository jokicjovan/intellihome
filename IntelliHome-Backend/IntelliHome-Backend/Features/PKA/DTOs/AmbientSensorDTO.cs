using Data.Models.Shared;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AmbientSensorDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public SmartDeviceCategory Category { get; set; }
        public SmartDeviceType Type { get; set; }
        public Boolean IsConnected { get; set; }
        public Boolean IsOn { get; set; }
        public Double Temperature { get; set; }
        public Double Humidity { get; set; }
        public Double PowerPerHour { get; set; }
    }
}
