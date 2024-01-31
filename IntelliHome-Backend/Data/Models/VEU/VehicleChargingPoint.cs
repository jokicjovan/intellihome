using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class VehicleChargingPoint : IBaseEntity
    {
        public Guid Id { get; set; }
        public Boolean IsFree { get; set; }
        public double? InitialCapacity { get; set; }
        public double? Capacity { get; set; }
        public double? ChargeLimit { get; set; }
        public String? Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [JsonIgnore]
        public VehicleCharger VehicleCharger { get; set; }

        public VehicleChargingPoint()
        {
        }
    }
}
