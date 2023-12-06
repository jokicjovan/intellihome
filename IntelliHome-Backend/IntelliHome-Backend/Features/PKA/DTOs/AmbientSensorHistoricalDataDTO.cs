namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AmbientSensorHistoricalDataDTO
    {
        public DateTime Timestamp { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}
