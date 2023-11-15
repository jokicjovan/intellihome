using Data.Models.VEU;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class SolarPanelSystemService : ISolarPanelSystemService
    {
        private readonly ISolarPanelSystemRepository _solarPanelSystemRepository;
        private readonly IDeviceConnectionService _deviceConnectionService;

        public SolarPanelSystemService(ISolarPanelSystemRepository solarPanelSystemRepository, IDeviceConnectionService deviceConnectionService)
        {
            _solarPanelSystemRepository = solarPanelSystemRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<SolarPanelSystem> CreateSolarPanelSystem(SolarPanelSystem solarPanelSystem)
        {
            solarPanelSystem = await _solarPanelSystemRepository.Create(solarPanelSystem);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(solarPanelSystem);
            if (success)
            {
                solarPanelSystem.IsConnected = true;
                await _solarPanelSystemRepository.Update(solarPanelSystem);
            }
            return solarPanelSystem;
        }

        public async Task<SolarPanelSystem> GetSolarPanelSystem(Guid Id)
        {
            SolarPanelSystem solarPanelSystem = await _solarPanelSystemRepository.Read(Id);
            if (solarPanelSystem == null)
            {
                throw new ResourceNotFoundException("Solar panel system with provided Id not found!");
            }
            return solarPanelSystem;
        }
    }
}
