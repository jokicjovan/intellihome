using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class LampCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "Brightness limit is required.")]
        public Double BrightnessLimit { get; set; }

        public LampCreationDTO() { }
    }
}
