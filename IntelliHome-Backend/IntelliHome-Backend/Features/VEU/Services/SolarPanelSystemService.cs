using Data.Models.VEU;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;

namespace IntelliHome_Backend.Features.VEU.Services
{
    public class SolarPanelSystemService : ISolarPanelSystemService
    {
        private readonly ISolarPanelSystemRepository _solarPanelSystemRepository;

        public SolarPanelSystemService(ISolarPanelSystemRepository solarPanelSystemRepository)
        {
            _solarPanelSystemRepository = solarPanelSystemRepository;
        }

        public Task<SolarPanelSystem> CreateSolarPanelSystem(SolarPanelSystem solarPanelSystem)
        {
            return _solarPanelSystemRepository.Create(solarPanelSystem);
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
