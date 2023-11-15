using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.Shared.DTOs
{
    public class SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public IFormFile? Image { get; set; }
    }
}
