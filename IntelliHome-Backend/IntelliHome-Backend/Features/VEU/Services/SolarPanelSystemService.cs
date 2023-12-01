using Data.Models.VEU;
using IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.VEU.Repositories;
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

        public async Task<SolarPanelSystem> Create(SolarPanelSystem entity)
        {
            entity = await _solarPanelSystemRepository.Create(entity);
            //bool success = await _deviceConnectionService.ConnectWithSmartDevice(entity);
            //if (success)
            //{
            //    entity.IsConnected = true;
            //    await _solarPanelSystemRepository.Update(entity);
            //}
            return entity;
        }

        public Task<SolarPanelSystem> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<SolarPanelSystem> Get(Guid id)
        {
            SolarPanelSystem solarPanelSystem = await _solarPanelSystemRepository.Read(id);
            if (solarPanelSystem == null)
            {
                throw new ResourceNotFoundException("Solar panel system with provided Id not found!");
            }
            return solarPanelSystem;
        }

        public Task<IEnumerable<SolarPanelSystem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SolarPanelSystem> Update(SolarPanelSystem entity)
        {
            throw new NotImplementedException();
        }
    }
}
