using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Communications.Services.Interfaces
{
    public interface ISimulationService
    {
        public Task<bool> AddDeviceToSimulator(SmartDevice smartDevice);
        public Task AddDevicesFromDatabaseToSimulator();
    }
}
