using IntelliHome_Backend.Features.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.VEU.DTOs
{
    public class VehicleChargerCreationDTO : SmartDeviceCreationDTO
    {
        [Required(ErrorMessage = "Power is required.")]
        [Range(1, 1000, ErrorMessage = "Power should be between 1.4KW and 1000KW")]
        public Double Power { get; set; }

        [Required(ErrorMessage = "Power is required.")]
        [Range(1, 4, ErrorMessage = "Number of charging points should be between 1 and 4")]
        public Int16 NumberOfChargingPoints{ get; set; }

        public VehicleChargerCreationDTO() { }
    }
}
