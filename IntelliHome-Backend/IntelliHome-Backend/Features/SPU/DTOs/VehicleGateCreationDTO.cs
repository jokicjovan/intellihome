using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.SPU.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class VehicleGateCreationDTO : SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "At least one allowed licence plate is required.")]
        [RegexStringListValidator(@"^[A-Z]{2}\d{3,5}[A-Z]{2}$")]
        public List<String> AllowedLicencePlates { get; set; }


        [Required(ErrorMessage = "PowerPerHour is required.")]
        public double PowerPerHour { get; set; }

        public VehicleGateCreationDTO() 
        {
            AllowedLicencePlates = new List<string>();
        }
    }
}
