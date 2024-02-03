using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AirConditionerCreationDTO : SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "Minimum temperature is required.")]
        [Range(10, 20, ErrorMessage = "Minimum temperature should be between 10 and 20")]
        public double MinTemperature { get; set; }

        [Required(ErrorMessage = "Maximum temperature is required.")]
        [Range(25, 35, ErrorMessage = "Maximum temperature should be between 25 and 30")]
        public double MaxTemperature { get; set; }

        [Required(ErrorMessage = "Modes list is required.")]
        [MinLength(1, ErrorMessage = "At least one mode is required.")]
        public List<AirConditionerMode> Modes { get; set; }

        [Required(ErrorMessage = "Power per hour is required.")]
        [Range(0, 1000, ErrorMessage = "Power per hour should be between 1 and 1000 KWh")]
        public double PowerPerHour { get; set; }

        public AirConditionerCreationDTO() {
            Modes = new List<AirConditionerMode>();
        }
    }
}
