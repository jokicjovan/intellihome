using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IAmbientSensorService : ICrudService<AmbientSensor>
    {
        IEnumerable<AmbientSensor> GetAllWithHome();
    }
}
