using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Double VehicleBatteryCapacity { get; set; }
        public Double VehicleBatteryCurrentCapacity { get; set; }
        [Range(0,100)]
        public Int16 VehicleBatteryPercetingeLimit { get; set; }

        public VehicleChargingPoint()
        {
        }
    }
}
