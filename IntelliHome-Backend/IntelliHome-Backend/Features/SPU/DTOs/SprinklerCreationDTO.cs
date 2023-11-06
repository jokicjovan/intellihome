using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class SprinklerCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "PowerPerHour is required.")]
        public double PowerPerHour { get; set; }

        public SprinklerCreationDTO()
        {
        }

    }
}
