using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Communications.Services.Interfaces
{
    public interface IHeartbeatService
    {
        public Task SetupLastWillHandler();
    }
}
