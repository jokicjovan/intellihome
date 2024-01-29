namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public class VehicleChargingPointDTO
    {
        public Guid Id { get; set; }
        public Boolean IsFree { get; set; }
        public double InitialCapacity { get; set; }
        public double MaxCapacity { get; set; }
        public double ChargeLimit { get; set; }
        public double CurrentCapacity { get; set; }
        public String Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
