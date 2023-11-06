using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class BatteryCreationDTO
    {
        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 100, ErrorMessage = "Capacity should be between 1 and 100 kWh")]
        public Double Capacity { get; set; }
    }
}
