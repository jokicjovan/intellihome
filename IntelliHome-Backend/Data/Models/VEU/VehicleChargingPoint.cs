using System.ComponentModel.DataAnnotations.Schema;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class VehicleChargingPoint : IBaseEntity
    {
        public Guid Id { get; set; }
        [NotMapped]
        public Boolean IsFree { get; set; } = false;
        public VehicleCharger VehicleCharger { get; set; }

        public VehicleChargingPoint()
        {
        }
    }
}
