using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.SPU.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class VehicleGateCreationDTO : SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "Allowed licence plate list is required.")]
        [RegexStringListValidator(@"^[A-Z]{2}\d{3,5}[A-Z]{2}$")]
        [MinLength(1, ErrorMessage = "At least one licence plate is required.")]
        public List<String> AllowedLicencePlates { get; set; }

        [Required(ErrorMessage = "Power per hour is required.")]
        [Range(0, 1000, ErrorMessage = "Power per hour should be between 1 and 1000 KWh")]
        public double PowerPerHour { get; set; }

        public VehicleGateCreationDTO() 
        {
            AllowedLicencePlates = new List<string>();
        }
    }
}
