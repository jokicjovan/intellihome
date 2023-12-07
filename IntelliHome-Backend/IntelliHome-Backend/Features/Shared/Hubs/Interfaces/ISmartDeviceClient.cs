namespace IntelliHome_Backend.Features.Shared.Hubs.Interfaces
{
    public interface ISmartDeviceClient
    {
        Task ReceiveSmartDeviceData(object data);
        Task ReceiveSmartDeviceSubscriptionResult(string result);
    }
}
