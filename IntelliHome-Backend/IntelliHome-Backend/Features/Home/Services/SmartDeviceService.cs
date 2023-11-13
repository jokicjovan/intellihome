using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;

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
    }
}
