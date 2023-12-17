using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AirConditionerDTO: SmartDeviceDTO
    {
        public double CurrentTemperature { get; set; }
        public string Mode{ get; set; }
        public Double PowerPerHour { get; set; }
        public List<string> Modes { get; set; }

        public List<AirConditionerScheduleDTO> Schedules { get; set; }

        public Double MinTemp { get; set; }

        public Double MaxTemp { get; set; }
    }
}
