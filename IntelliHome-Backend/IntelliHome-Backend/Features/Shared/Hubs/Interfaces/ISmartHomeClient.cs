namespace IntelliHome_Backend.Features.Shared.Hubs.Interfaces
{
    public interface ISmartHomeClient
    {
        Task ReceiveSmartHomeSubscriptionResult(string result);
        Task ReceiveSmartHomeUsageData(object data);
    }
}
