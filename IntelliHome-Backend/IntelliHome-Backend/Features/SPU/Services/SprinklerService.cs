using Data.Models.SPU;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class SprinklerService : ISprinklerService
    {
        private readonly ISprinklerRepository _sprinklerRepository;
        private readonly IDeviceConnectionService _deviceConnectionService;

        public SprinklerService(ISprinklerRepository sprinklerRepository, IDeviceConnectionService deviceConnectionService)
        {
            _sprinklerRepository = sprinklerRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<Sprinkler> CreateSprinkler(Sprinkler sprinkler)
        {
            sprinkler = await _sprinklerRepository.Create(sprinkler);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(sprinkler);
            if (success)
            {
                sprinkler.IsConnected = true;
                await _sprinklerRepository.Update(sprinkler);
            }
            return sprinkler;
        }
    }
}
