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

        public IEnumerable<SmartDevice> GetAllSmartDevices()
        {
            return _smartDeviceRepository.FindAllSmartDevices();
        }
    }
}
