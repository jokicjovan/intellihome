using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class WashingMachineDTO : SmartDeviceDTO
    {
        public List<String> Modes { get; set; }
        public String Mode { get; set; }

        public List<WashingMachineScheduleDTO>? Schedules { get; set; }
        public DateTime StartTime { get; set; }
        public Double PowerPerHour { get; set; }
        public Double CurrentTemperature { get; set; }
    }
}
