using Data.Models.Home;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;

namespace IntelliHome_Backend.Features.Home.Services
{
    public class SmartHomeService : ISmartHomeService
    {
        private readonly ISmartHomeRepository _smartHomeRepository;

        public SmartHomeService(ISmartHomeRepository smartHomeRepository)
        {
            _smartHomeRepository = smartHomeRepository;
        }

        public async Task<SmartHome> GetSmartHome(Guid id)
        {
            SmartHome smartHome = await _smartHomeRepository.Read(id);
            if (smartHome == null)
            {
                throw new ResourceNotFoundException("Smart house with provided Id not found!");
            }
            return smartHome;
        }
    }
}
