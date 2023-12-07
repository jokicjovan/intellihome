using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AmbientSensorDTO : SmartDeviceDTO
    {
        public Double Temperature { get; set; }
        public Double Humidity { get; set; }
        public Double PowerPerHour { get; set; }
    }
}
