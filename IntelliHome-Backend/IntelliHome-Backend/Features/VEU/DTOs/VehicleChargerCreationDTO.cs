using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class VehicleChargerCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "Power is required.")]
        [Range(1.4, 1000, ErrorMessage = "Power should be between 1.4KW and 1000KW")]
        public Double Power { get; set; }

        public VehicleChargerCreationDTO() { }
    }
}
