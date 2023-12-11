using Data.Models.PKA;
using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Home.Handlers.Interfaces
{
    public interface ISmartDeviceHandler
    {
        void PublishMessageToSmartDevice(SmartDevice smartDevice, string payload);
        Task<bool> ConnectToSmartDevice(SmartDevice smartDevice, Dictionary<string, object> additionalAttributes);
        void SubscribeToSmartDevice(SmartDevice smartDevice);
        Task TurnOnSmartDevice(SmartDevice smartDevice, bool turnOn);
    }
}
