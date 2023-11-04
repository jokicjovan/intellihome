using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AirConditionerCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "Minimum temperature is required.")]
        public double MinTemperature { get; set; }

        [Required(ErrorMessage = "Maximum temperature is required.")]
        public double MaxTemperature { get; set; }

        [Required(ErrorMessage = "Modes are required.")]
        public List<AirConditionerMode> Modes { get; set; }

        public AirConditionerCreationDTO() {
            Modes = new List<AirConditionerMode>();
        }
    }
}
