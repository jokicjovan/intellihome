using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AmbientSensorCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "PowerPerHour is required.")]
        public double PowerPerHour { get; set; }
    }
}
