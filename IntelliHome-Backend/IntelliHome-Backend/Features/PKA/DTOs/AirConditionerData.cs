using Data.Models.PKA;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AirConditionerData
    {
            public Double Temperature { get; set; }
            public string Mode { get; set; }
            public Double ConsumptionPerMinute { get; set; }
            public DateTime? Timestamp { get; set; }
    }
}
