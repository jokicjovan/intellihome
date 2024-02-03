using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargerDTO : SmartDeviceDTO
    {
        public Double PowerPerHour { get; set; }
        public List<VehicleChargingPointDTO> ChargingPoints { get; set; } = new List<VehicleChargingPointDTO>();
    }
}
