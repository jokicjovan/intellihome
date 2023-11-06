using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class SolarPanelCreationDTO
    {
        [Required(ErrorMessage = "Area is required.")]
        [Range(1, 20, ErrorMessage = "Area should be between 1 and 20 m^2")]
        public Double Area { get; set; }

        [Required(ErrorMessage = "Efficiency is required.")]
        [Range(1, 100, ErrorMessage = "Efficiency should be between 1 and 100 %")]
        public Double Efficiency { get; set; }

        public SolarPanelCreationDTO() { }
    }
}
