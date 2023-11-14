using Data.Models.PKA;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class LampService : ILampService
    {
        private readonly ILampRepository _lampRepository;
        private readonly IDeviceConnectionService _deviceConnectionService;

        public LampService(ILampRepository lampRepository, IDeviceConnectionService deviceConnectionService)
        {
            _lampRepository = lampRepository;
            _deviceConnectionService = deviceConnectionService;
        }

        public async Task<Lamp> CreateLamp(Lamp lamp)
        {
            lamp = await _lampRepository.Create(lamp);
            bool success = await _deviceConnectionService.ConnectWithSmartDevice(lamp);
            if (success)
            {
                lamp.IsConnected = true;
                await _lampRepository.Update(lamp);
            }
            return lamp;
        }
    }
}
