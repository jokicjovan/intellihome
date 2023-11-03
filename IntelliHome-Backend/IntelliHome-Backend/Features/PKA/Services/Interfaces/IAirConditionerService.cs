using Data.Models.PKA;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IAirConditionerService
    {
        Task<AirConditioner> CreateAirConditioner(AirConditioner airConditioner);
    }
}
