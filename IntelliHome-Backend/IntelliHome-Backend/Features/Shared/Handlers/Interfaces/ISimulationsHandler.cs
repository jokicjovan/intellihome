using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Shared.Handlers.Interfaces
{
    public interface ISimulationsHandler
    {
        Task<bool> AddDeviceToSimulator(object deviceRequestBody);
        Task<bool> TurnOnDeviceSimulator(Guid deviceId);
        Task AddDevicesFromDatabaseToSimulator();
    }
}
