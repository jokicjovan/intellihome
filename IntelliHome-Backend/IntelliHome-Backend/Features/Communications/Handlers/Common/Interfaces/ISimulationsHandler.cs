using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces
{
    public interface ISimulationsHandler
    {
        public Task<bool> AddDeviceToSimulator(SmartDevice smartDevice);
        public Task AddDevicesFromDatabaseToSimulator();
    }
}
