using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem
{
    public class SolarPanelSystemCreationDTO : SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "Area is required.")]
        [Range(5, 10000, ErrorMessage = "Area should be between 5 and 10000 m^2")]
        public double Area { get; set; }

        [Required(ErrorMessage = "Efficiency is required.")]
        [Range(1, 100, ErrorMessage = "Efficiency should be between 1 and 100 %")]
        public double Efficiency { get; set; }

        public SolarPanelSystemCreationDTO() { }
    }
}
