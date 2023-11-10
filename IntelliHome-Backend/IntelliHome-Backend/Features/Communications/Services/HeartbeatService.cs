﻿using Data.Models.Shared;
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

        public async Task ToggleDeviceSimulator(SmartDevice smartDevice, bool turnOn = true)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string baseUrl = "http://127.0.0.1:8000/";
                    string endpoint = turnOn ? "start-simulation" : "stop-simulation";
                    string apiUrl = baseUrl + endpoint;

                    var content = new StringContent(string.Empty);
                    HttpResponseMessage response = await client.PostAsync($"{apiUrl}/{smartDevice.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("API Response:");
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"API Request failed with status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"API Request failed: {ex.Message}");
                }
            };
        }

        public async Task SetupSimulatorsFromDatabase(bool turnOn = true)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();

                IEnumerable<SmartDevice> smartDevices = smartDeviceService.GetAllSmartDevices();
                var tasks = smartDevices.Select(smartDevice => ToggleDeviceSimulator(smartDevice, turnOn));
                await Task.WhenAll(tasks);
            }
        }

        public async Task SetupHeartBeatTrackerAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IMqttClient mqttClient = scope.ServiceProvider.GetRequiredService<IMqttClient>();
                var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();
                await mqttClient.ConnectAsync(options);

                await mqttClient.SubscribeAsync(new MqttTopicFilter { Topic = $"Heartbeat" });
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
                    return Task.CompletedTask;
                };
            }
        }
    }
}
