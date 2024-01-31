using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class VehicleCharger : SmartDevice
    {
        public Double PowerPerHour { get; set; }
        public List<VehicleChargingPoint> ChargingPoints { get; set; }

        public VehicleCharger()
        {
            ChargingPoints = new List<VehicleChargingPoint>();
        }
    }
}
