using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AmbientSensorCreationDTO : SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "Power per hour is required.")]
        [Range(0, 1000, ErrorMessage = "Power per hour should be between 1 and 1000 KWh")]
        public double PowerPerHour { get; set; }
    }
}
