using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargingPointCreationDTO
    {
        [Required(ErrorMessage = "Initial capacity is required.")]
        [Range(10, 200, ErrorMessage = "Initial capacity should be between 10 and 200")]
        public double InitialCapacity { get; set; }

        [Required(ErrorMessage = "Maximum capacity is required.")]
        [Range(10, 200, ErrorMessage = "Maximum capacity should be between 10 and 200")]
        public double Capacity { get; set; }

        [Required(ErrorMessage = "Charge limit is required.")]
        [Range(0.0, 1.0, ErrorMessage = "Charge limit should be between 0.0 and 1.0")]
        public double ChargeLimit { get; set; }
    }
}
