using Data.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ISmartDeviceService
    {
        Task<SmartDevice> GetSmartDevice(Guid smartDeviceId);
        Task<SmartDevice> UpdateSmartDevice(SmartDevice smartDevice);
        IEnumerable<SmartDevice> GetAllSmartDevices();
        IEnumerable<SmartDevice> UpdateAllSmartDevices(List<SmartDevice> smartDevices);
        Task<(IEnumerable<SmartDevice>, Int32)> GetPagedSmartDevicesForSmartHome(Guid smartHomeId, int page, int pageSize);
        IEnumerable<SmartDevice> GetSmartDevicesForSmartHome(Guid smartHomeId);
    }
}
