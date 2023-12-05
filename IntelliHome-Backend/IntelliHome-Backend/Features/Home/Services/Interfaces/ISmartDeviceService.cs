using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ISmartDeviceService : ICrudService<SmartDevice>
    {
        IEnumerable<SmartDevice> GetAllWithHome();
        IEnumerable<SmartDevice> UpdateAll(List<SmartDevice> smartDevices);
        Task<(IEnumerable<SmartDevice>, Int32)> GetPagedSmartDevicesForSmartHome(Guid smartHomeId, int page, int pageSize);
        IEnumerable<SmartDevice> GetSmartDevicesForSmartHome(Guid smartHomeId);
        Task<bool> IsUserAllowed(Guid smartDeviceId, Guid userId);
    }
}
