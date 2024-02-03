namespace IntelliHome_Backend.Features.Shared.Hubs.Interfaces
{
    public interface ISmartDeviceClient
    {
        Task ReceiveSmartDeviceSubscriptionResult(string result);
        Task ReceiveSmartDeviceData(object data);
    }
}
