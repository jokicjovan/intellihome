using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Home.Services
{
    public class SmartDeviceService : ISmartDeviceService
    {
        private readonly ISmartDeviceRepository _smartDeviceRepository;

        public SmartDeviceService(ISmartDeviceRepository smartDeviceRepository)
        {
            _smartDeviceRepository = smartDeviceRepository;
        }

        public Task<SmartDevice> GetSmartDevice(Guid smartDeviceId)
        {
            return _smartDeviceRepository.Read(smartDeviceId);
        }

        public Task<SmartDevice> UpdateSmartDevice(SmartDevice smartDevice)
        {
            return _smartDeviceRepository.Update(smartDevice);
        }
    
        public IEnumerable<SmartDevice> GetAllSmartDevices()
        {
            return _smartDeviceRepository.FindAllWIthHome();
        }

        public IEnumerable<SmartDevice> UpdateAllSmartDevices(List<SmartDevice> smartDevices) 
        {
            return _smartDeviceRepository.UpdateAll(smartDevices);
        }

        public async Task<(IEnumerable<SmartDevice>, Int32)> GetPagedSmartDevicesForSmartHome(Guid smartHomeId, int page, int pageSize)
        {
            IQueryable<SmartDevice> query = GetSmartDevicesForSmartHome(smartHomeId).AsQueryable();
            Int32 totalItems = await query.CountAsync();
            //Int32 totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            IEnumerable<SmartDevice> entities = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (entities, totalItems);
        }

        public IEnumerable<SmartDevice> GetSmartDevicesForSmartHome(Guid smartHomeId) {
            return _smartDeviceRepository.FindSmartDevicesForSmartHome(smartHomeId);
        }
    }
}
