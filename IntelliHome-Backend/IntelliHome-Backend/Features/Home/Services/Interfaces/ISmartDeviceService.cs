using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ISmartDeviceService
    {
        Task<SmartDevice> GetSmartDevice(Guid smartDeviceId);
        Task<SmartDevice> UpdateSmartDevices(SmartDevice smartDevice);
        IEnumerable<SmartDevice> GetAllSmartDevices();
        IEnumerable<SmartDevice> UpdateAllSmartDevices(List<SmartDevice> smartDevices);
    }
}
