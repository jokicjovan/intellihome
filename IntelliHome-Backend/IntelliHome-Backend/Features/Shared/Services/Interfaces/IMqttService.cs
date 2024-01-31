using MQTTnet.Client;

namespace IntelliHome_Backend.Features.Shared.Services.Interfaces
{
    public interface IMqttService
    {
        Task PublishAsync(string topic, string payload);
        Task SubscribeAsync(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> messageHandler);
        Task ConnectAsync(string host, int port, CancellationToken cancellationToken = default);
    }
}
