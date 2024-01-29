using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class SprinklerDTO : SmartDeviceDTO
    {
        public Boolean IsSpraying { get; set; }
        public Double PowerPerHour { get; set; }
        public List<SprinklerScheduleDTO> Schedules { get; set; }
    }
}
