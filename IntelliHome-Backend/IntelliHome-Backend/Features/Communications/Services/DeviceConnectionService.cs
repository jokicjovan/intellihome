using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;

namespace IntelliHome_Backend.Features.Communications.Services
{
    public class DeviceConnectionService : IDeviceConnectionService
    {
        private readonly ISimulationService _simulationService;

        public DeviceConnectionService(ISimulationService simulationService)
        {
            _simulationService = simulationService;
        }

        public Task<bool> ConnectWithSmartDevice(SmartDevice smartDevice) {
            return _simulationService.AddDeviceToSimulator(smartDevice);
        }
    }
}
