using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ISmartDeviceService
    {
        IEnumerable<SmartDevice> GetAllSmartDevices();
    }
}
