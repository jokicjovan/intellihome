using MQTTnet.Client;

namespace IntelliHome_Backend.Features.Communications.Services.Interfaces
{
    public interface IMqttService
    {
        Task PublishAsync(string topic, string payload);
        Task SubscribeAsync(string topic, Action<MqttApplicationMessageReceivedEventArgs> messageHandler);
    }
}
