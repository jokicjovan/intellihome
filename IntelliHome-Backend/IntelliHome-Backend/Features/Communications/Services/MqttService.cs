using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Packets;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;

namespace IntelliHome_Backend.Features.Communications.Services
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _mqttClient;

        public MqttService(IMqttClient mqttClient)
        {
            _mqttClient = mqttClient;
        }

        public async Task ConnectAsync(string host, int port, CancellationToken cancellationToken = default)
        {
            if (_mqttClient.IsConnected)
            {
                throw new InvalidOperationException("MqttService is already initialized.");
            }

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(host, port)
                .Build();

            await _mqttClient.ConnectAsync(options, cancellationToken);
        }

        public async Task PublishAsync(string topic, string payload)
        {
            if (_mqttClient.IsConnected)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .Build();

                await _mqttClient.PublishAsync(message);
            }
            else
            {
                // Handle not connected scenario
            }
        }

        public async Task SubscribeAsync(string topic, Action<MqttApplicationMessageReceivedEventArgs> messageHandler)
        {
            await _mqttClient.SubscribeAsync(new MqttTopicFilter { Topic = topic });
            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                if (e.ApplicationMessage.Topic == topic)
                {
                    try
                    {
                        messageHandler(e);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            };
        }
    }

}
