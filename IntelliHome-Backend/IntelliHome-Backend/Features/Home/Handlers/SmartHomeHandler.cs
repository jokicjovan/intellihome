using IntelliHome_Backend.Features.Home.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MQTTnet.Client;
using Data.Models.Home;
using Data.Models.Shared;
using IntelliHome_Backend.Features.VEU.DTOs;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using MQTTnet;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Home.DTOs;

namespace IntelliHome_Backend.Features.Home.Handlers
{
    public class SmartHomeHandler : ISmartHomeHandler
    {
        protected readonly IMqttService mqttService;
        protected readonly IServiceProvider serviceProvider;
        protected readonly IHubContext<SmartHomeHub, ISmartHomeClient> smartHomeHubContext;

        public SmartHomeHandler(IMqttService mqttService, IServiceProvider serviceProvider, IHubContext<SmartHomeHub, ISmartHomeClient> smartHomeHubContext)
        {
            this.mqttService = mqttService;
            this.serviceProvider = serviceProvider;
            this.smartHomeHubContext = smartHomeHubContext;
            this.mqttService.SubscribeAsync($"FromSmartHome/+/Usage", HandleMessageFromHome);
        }

        public async void SubscribeToSmartHome(SmartHome smartHome)
        {
            string topic = $"FromSmartHome/{smartHome.Id}";
            await mqttService.SubscribeAsync(topic, HandleMessageFromHome);
        }

        public async void PublishMessageToSmartHome(SmartHome smartHome, string payload)
        {
            string topic = $"ToSmartHome/{smartHome.Id}";
            await mqttService.PublishAsync(topic, payload);
        }

        protected async Task HandleMessageFromHome(MqttApplicationMessageReceivedEventArgs e)
        {
            String[] topic_parts = e.ApplicationMessage.Topic.Split('/');
            if (topic_parts.Length < 3)
            {
                Console.WriteLine("Error handling topic");
                return;
            }
            string smartHomeId = topic_parts[1];
            _ = smartHomeHubContext.Clients.Group(smartHomeId).ReceiveSmartHomeUsageData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();
            var smartHomeService = scope.ServiceProvider.GetRequiredService<ISmartHomeService>();
            var smartHome = await smartHomeService.Get(Guid.Parse(smartHomeId));
            if (smartHome != null)
            {
                var smartHomeUsageData = JsonConvert.DeserializeObject<SmartHomeUsageDataDTO>(e.ApplicationMessage.ConvertPayloadToString());
                var smartHomeUsageDataInflux = new Dictionary<string, object>
                    {
                        { "production_per_minute", smartHomeUsageData.ProductionPerMinute },
                        { "consumption_per_minute", smartHomeUsageData.ConsumptionPerMinute },
                        { "grid_per_minute", smartHomeUsageData.GridPerMinute }
                    };
                var smartHomeUsageDataTags = new Dictionary<string, string>
                    {
                        { "device_id", smartHome.Id.ToString() }
                    };
                //smartHomeService.AddUsageMeasurement(smartHomeUsageDataInflux, smartHomeUsageDataTags);
            }
        }
    }
}
