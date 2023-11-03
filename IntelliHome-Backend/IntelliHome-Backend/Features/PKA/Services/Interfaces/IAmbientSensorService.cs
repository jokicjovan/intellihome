using Data.Models.PKA;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IAmbientSensorService
    {
        Task<AmbientSensor> CreateAmbientSensor(AmbientSensor ambientSensor);
    }
}
