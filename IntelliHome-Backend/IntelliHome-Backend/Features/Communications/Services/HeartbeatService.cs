using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Server;

namespace IntelliHome_Backend.Features.Communications.Services
{
    public class HeartbeatService : IHeartbeatService
    {
        private readonly IServiceProvider _serviceProvider;
        public HeartbeatService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SetupLastWillHandler()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IMqttClient mqttClient = scope.ServiceProvider.GetRequiredService<IMqttClient>();
                var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();
                await mqttClient.ConnectAsync(options);

                await mqttClient.SubscribeAsync(new MqttTopicFilter { Topic = "will" });
                mqttClient.ApplicationMessageReceivedAsync += async e =>
                {
                    using (var scope = _serviceProvider.CreateScope()) 
                    {
                        try
                        {
                            ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();
                            Guid deviceId = Guid.Parse(e.ApplicationMessage.ConvertPayloadToString());
                            SmartDevice smartDevice = await smartDeviceService.GetSmartDevice(deviceId);
                            smartDevice.IsConnected = false;
                            await smartDeviceService.UpdateSmartDevice(smartDevice);
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
}
