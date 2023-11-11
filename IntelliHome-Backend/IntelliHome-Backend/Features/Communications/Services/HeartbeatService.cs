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

        public async Task<bool> ToggleDeviceSimulator(SmartDevice smartDevice, bool turnOn = true)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string baseUrl = "http://127.0.0.1:8000/";
                    string endpoint = turnOn ? "add-device" : "remove-device";
                    string apiUrl = baseUrl + endpoint;

                    var content = new StringContent(string.Empty);
                    HttpResponseMessage response = await client.PostAsync($"{apiUrl}/{smartDevice.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return false;
                }
            };
        }

        public async Task SetupLastWillHandler(Guid smartDeviceId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IMqttClient mqttClient = scope.ServiceProvider.GetRequiredService<IMqttClient>();
                var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();
                await mqttClient.ConnectAsync(options);

                await mqttClient.SubscribeAsync(new MqttTopicFilter { Topic = $"{smartDeviceId}/will" });
                mqttClient.ApplicationMessageReceivedAsync += async e =>
                {
                    using (var scope = _serviceProvider.CreateScope()) 
                    {
                        ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();
                        Guid deviceId = Guid.Parse(e.ApplicationMessage.Topic.Split("/")[0]);
                        SmartDevice smartDevice = await smartDeviceService.GetSmartDevice(smartDeviceId);
                        smartDevice.IsConnected = false;
                        await smartDeviceService.UpdateSmartDevices(smartDevice);
                    }
                };
            }
        }

        public async Task SetupSimulatorsFromDatabase(bool turnOn = true)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();

                List<SmartDevice> smartDevices = smartDeviceService.GetAllSmartDevices().ToList();
                foreach (SmartDevice smartDevice in smartDevices)
                {
                    bool success = await ToggleDeviceSimulator(smartDevice, turnOn);
                    smartDevice.IsConnected = success;
                    if (success)
                    {
                        SetupLastWillHandler(smartDevice.Id);
                    }
                }
                smartDeviceService.UpdateAllSmartDevices(smartDevices);
            }
        }
    }
}
