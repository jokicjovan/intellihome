using Data.Models.Home;

namespace IntelliHome_Backend.Features.Home.Services.Interfaces
{
    public interface ISmartHomeService
    {
        public Task<SmartHome> GetSmartHome(Guid Id);
    }
}
