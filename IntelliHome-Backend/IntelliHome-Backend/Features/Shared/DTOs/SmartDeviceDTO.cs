using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.Shared.DTOs
{
    public class SmartDeviceDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string Image { get; set; }

        [Required(ErrorMessage = "PowerPerHour is required.")]
        public double PowerPerHour { get; set; }
    }
}
