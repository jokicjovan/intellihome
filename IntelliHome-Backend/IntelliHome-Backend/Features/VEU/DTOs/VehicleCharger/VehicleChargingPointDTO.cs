namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargingPointDTO
    {
        public Guid Id { get; set; }
        public Boolean IsFree { get; set; }
        public Double? InitialCapacity { get; set; }
        public Double? Capacity { get; set; }
        public Double? ChargeLimit { get; set; }
        public Double CurrentCapacity { get; set; }
        public String? Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
