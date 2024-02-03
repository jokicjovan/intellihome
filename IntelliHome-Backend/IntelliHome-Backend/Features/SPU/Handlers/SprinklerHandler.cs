using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using IntelliHome_Backend.Features.Home.Handlers;
using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using System.Globalization;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.Handlers
{
    public class SprinklerHandler : SmartDeviceHandler, ISprinklerHandler
    {
        public SprinklerHandler(IConfiguration configuration, MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(configuration, mqttFactory, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.SPU}/{SmartDeviceType.SPRINKLER}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();
            var sprinklerService = scope.ServiceProvider.GetRequiredService<ISprinklerService>();

            var sprinkler = await sprinklerService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));

            if (sprinkler != null)
            {
                var sprinklerData = JsonConvert.DeserializeObject<SprinklerData>(e.ApplicationMessage.ConvertPayloadToString());
                var sprinklerDataInflux = new Dictionary<string, object>
                {
                    { "isSpraying", sprinklerData.IsSpraying},
                };
                var sprinklerDataTags = new Dictionary<string, string>
                {
                    { "deviceId", sprinkler.Id.ToString() },
                };
                sprinklerService.AddPoint(sprinklerDataInflux, sprinklerDataTags);
            }
        }

        public override Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            var scope = serviceProvider.CreateScope();  
            var sprinklerService = scope.ServiceProvider.GetRequiredService<ISprinklerService>();

            Sprinkler sprinkler = sprinklerService.GetWithSmartHome(smartDevice.Id).Result;
            var result = sprinkler.ScheduledWorks.SelectMany(work => work.DateTo != null 
                ? new[]
                {
                    new { timestamp = $"{work.DateFrom:dd/MM/yyyy} {work.Start:HH:mm}", set_spraying = 1 },
                    new { timestamp = $"{work.DateTo:dd/MM/yyyy} {work.End:HH:mm}", set_spraying = 0 }
                }
                : new[]
                {
                    new { timestamp = $"{work.DateFrom:dd/MM/yyyy} {work.Start:HH:mm}", set_spraying = 1 }
                }).ToList().Where(work => DateTime.ParseExact(work.timestamp, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) > DateTime.UtcNow).ToList();


            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
            {
                { "power_per_hour", sprinkler.PowerPerHour},
                { "schedule_list", result}
            };

            var requestBody = new
            {
                device_id = smartDevice.Id,
                smart_home_id = smartDevice.SmartHome.Id,
                device_category = smartDevice.Category.ToString(),
                device_type = smartDevice.Type.ToString(),
                host = configuration["MqttBroker:Host"],
                port = configuration["MqttBroker:Port"],
                keepalive = configuration["MqttBroker:Keepalive"],
                kwargs = additionalAttributes
            };
            return simualtionsHandler.AddDeviceToSimulator(requestBody);
        }


        public void SetSpraying(Sprinkler sprinkler, bool isSpraying)
        {
            string action = isSpraying ? $"turn_sprinkler_on" : $"turn_sprinkler_off";
            string payload = JsonConvert.SerializeObject(new { action });
            PublishMessageToSmartDevice(sprinkler, payload);
        }

        public void AddSchedule(Sprinkler sprinkler, string timestamp, bool set_spraying)
        {
            string action = $"add_schedule";
            string payload = JsonConvert.SerializeObject(new { action, timestamp, set_spraying});
            PublishMessageToSmartDevice(sprinkler, payload);
        }
    }
}
