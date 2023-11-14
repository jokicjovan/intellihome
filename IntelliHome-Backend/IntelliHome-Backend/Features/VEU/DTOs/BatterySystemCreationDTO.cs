using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class BatterySystemCreationDTO : SmartDeviceDTO
    {
        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 100, ErrorMessage = "Capacity should be between 1 and 100 kWh")]
        public Double Capacity { get; set; }
    }
}
