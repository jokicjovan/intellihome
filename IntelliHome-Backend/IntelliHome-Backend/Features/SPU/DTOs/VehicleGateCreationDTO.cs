using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.SPU.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class VehicleGateCreationDTO : SmartDeviceDTO
    {
        [Required]
        [RegexStringListValidator(@"^[A-Z]{2}\d{3,5}[A-Z]{2}$")]
        public List<String> AllowedLicencePlates { get; set; }

        public VehicleGateCreationDTO() 
        {
            AllowedLicencePlates = new List<string>();
        }
    }
}
