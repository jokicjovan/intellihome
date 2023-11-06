using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class VehicleCharger : SmartDevice
    {
        public Double Power { get; set; }
        public List<VehicleChargingPoint> ChargingPoints { get; set; }
        public Int32 NumberOfFreeChargingPoints => ChargingPoints.Count(x => x.IsFree == true);

        public VehicleCharger()
        {
            ChargingPoints = new List<VehicleChargingPoint>();
        }
    }
}
