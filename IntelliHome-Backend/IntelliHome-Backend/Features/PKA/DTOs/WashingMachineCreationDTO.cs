using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class WashingMachineCreationDTO : SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "Modes Ids list is required.")]
        [MinLength(1, ErrorMessage = "At least one mode is required.")]
        public List<Guid> ModesIds{ get; set; }

        [Required(ErrorMessage = "Power per hour is required.")]
        [Range(0, 1000, ErrorMessage = "Power per hour should be between 1 and 1000 KWh")]
        public double PowerPerHour { get; set; }

        public WashingMachineCreationDTO() {
            ModesIds = new List<Guid>();
        }
    }
}
