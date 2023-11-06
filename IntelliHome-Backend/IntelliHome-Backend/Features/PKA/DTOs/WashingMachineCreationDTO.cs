using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class WashingMachineCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "At least one mode Id is required.")]
        public List<Guid> ModesIds{ get; set; }

        [Required(ErrorMessage = "PowerPerHour is required.")]
        public double PowerPerHour { get; set; }

        public WashingMachineCreationDTO() {
            ModesIds = new List<Guid>();
        }
    }
}
