using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class SprinklerService : ISprinklerService
    {
        private readonly ISprinklerRepository _sprinklerRepository;
        public SprinklerService(ISprinklerRepository sprinklerRepository)
        {
            _sprinklerRepository = sprinklerRepository;
        }

        public Task<Sprinkler> CreateSprinkler(Sprinkler sprinkler)
        {
            return _sprinklerRepository.Create(sprinkler);
        }
    }
}
