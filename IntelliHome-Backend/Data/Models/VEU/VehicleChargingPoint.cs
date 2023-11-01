using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class VehicleChargingPoint : IBaseEntity
    {
        public Guid Id { get; set; }
        public Boolean IsFree { get; set; }
        public Decimal VehicleBatteryCapacity { get; set; }
        public Decimal VehicleBatteryCurrentCapacity { get; set; }
        public Int16 VehicleBatteryCapacityLimit { get; set; }

        public VehicleChargingPoint()
        {
        }
    }
}
