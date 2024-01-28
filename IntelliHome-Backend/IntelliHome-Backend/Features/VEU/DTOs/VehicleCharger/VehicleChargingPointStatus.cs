using System.Runtime.Serialization;

namespace IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger
{
    public enum VehicleChargingPointStatus
    {
        [EnumMember(Value = "CHARGING")]
        CHARGING,
        [EnumMember(Value = "FINISHED")]
        FINISHED
    }
}
