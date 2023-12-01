using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Repositories.Interfaces
{
    public interface IAmbientSensorRepository : ICrudRepository<AmbientSensor>
    {
        IEnumerable<AmbientSensor> FindAllWIthHome();
    }
}
