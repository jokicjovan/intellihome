using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class VehicleCharger : SmartDevice
    {
        public Decimal Power { get; set; }
        List<VehicleChargingPoint> ChargingPoints { get; set; }
        public Int32 FreeChargingPoints => ChargingPoints.Count(x => x.IsFree == true);

        public VehicleCharger()
        {
            ChargingPoints = new List<VehicleChargingPoint>();
        }
    }
}
