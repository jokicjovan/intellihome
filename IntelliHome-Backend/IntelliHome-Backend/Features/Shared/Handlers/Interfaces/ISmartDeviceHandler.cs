using Data.Models.PKA;
using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Shared.Handlers.Interfaces
{
    public interface ISmartDeviceHandler
    {
        void PublishMessageToSmartDevice(SmartDevice smartDevice, String payload);
        Task<bool> ConnectToSmartDevice(SmartDevice smartDevice, Dictionary<String, object> additionalAttributes);
        void SubscribeToSmartDevice(SmartDevice smartDevice);
        Task TurnOnSmartDevice(SmartDevice smartDevice, bool turnOn);
    }
}
