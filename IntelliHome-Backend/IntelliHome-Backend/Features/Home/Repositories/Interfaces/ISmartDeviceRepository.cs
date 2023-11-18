using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.Home.Repositories.Interfaces
{
    public interface ISmartDeviceRepository : ICrudRepository<SmartDevice>
    {
        IEnumerable<SmartDevice> FindAllWIthHome();
        IEnumerable<SmartDevice> UpdateAll(List<SmartDevice> smartDevices);
        IEnumerable<SmartDevice> FindSmartDevicesForSmartHome(Guid smartHomeId);
    }
}
