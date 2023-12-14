using Data.Models.PKA;
using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Home.Handlers.Interfaces
{
    public interface ISmartDeviceHandler
    {
        Task PublishMessageToSmartDevice(SmartDevice smartDevice, string payload);
        Task<bool> ConnectToSmartDevice(SmartDevice smartDevice, Dictionary<string, object> additionalAttributes);
        Task SubscribeToSmartDevice(SmartDevice smartDevice);
        Task ToggleSmartDevice(SmartDevice smartDevice, bool turnOn);
    }
}
