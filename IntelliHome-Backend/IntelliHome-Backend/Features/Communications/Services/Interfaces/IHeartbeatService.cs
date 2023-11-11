using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Communications.Services.Interfaces
{
    public interface IHeartbeatService
    {
        public Task<bool> ToggleDeviceSimulator(SmartDevice smartDevice, bool turnOn = true);
        public Task SetupSimulatorsFromDatabase(bool turnOn = true);
        public Task SetupLastWillHandler(Guid smartDeviceId);
    }
}
