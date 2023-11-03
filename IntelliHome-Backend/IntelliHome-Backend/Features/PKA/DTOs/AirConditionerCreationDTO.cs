using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;
using System.Text.Json.Serialization;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AirConditionerCreationDTO : SmartDeviceDTO
    {
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public List<AirConditionerMode> Modes { get; set; }

        public AirConditionerCreationDTO() {
            Modes = new List<AirConditionerMode>();
        }
    }
}
