using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class LampService : ILampService
    {
        private readonly ILampRepository _lampRepository;
        public LampService(ILampRepository lampRepository) 
        { 
            _lampRepository = lampRepository;
        }

        public Task<Lamp> CreateLamp(Lamp lamp)
        {
            return _lampRepository.Create(lamp);
        }
    }
}
