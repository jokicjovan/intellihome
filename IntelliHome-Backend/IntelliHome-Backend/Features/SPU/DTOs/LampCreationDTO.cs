using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class LampCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "Brightness limit is required.")]
        [Range(20, 1500, ErrorMessage = "Brightness limit should be between 20 and 1500 lumens")]
        public Double BrightnessLimit { get; set; }


        [Required(ErrorMessage = "PowerPerHour is required.")]
        public double PowerPerHour { get; set; }

        public LampCreationDTO() { }
    }
}
