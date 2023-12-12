﻿using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using IntelliHome_Backend.Features.Home.Handlers;
using Data.Models.SPU;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.SPU.Handlers
{
    public class VehicleGateHandler : SmartDeviceHandler, IVehicleGateHandler
    {
        public VehicleGateHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttService, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            this.mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.SPU}/{SmartDeviceType.VEHICLEGATE}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            smartDeviceHubContext.Clients.Group(e.ApplicationMessage.Topic.Split("/").Last()).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();

            var vehicleGateService = scope.ServiceProvider.GetRequiredService<IVehicleGateService>();

            var vehicleGate = await vehicleGateService.Get(Guid.Parse(e.ApplicationMessage.Topic.Split('/')[4]));

            if (vehicleGate != null)
            {
                var vehicleGateData = JsonConvert.DeserializeObject<VehicleGateData>(e.ApplicationMessage.ConvertPayloadToString());
                var vehicleGateDataInflux = new Dictionary<string, object>
                {
                        { "isPublic", vehicleGateData.IsPublic ? 1f : 0f },
                        { "licencePlate", vehicleGateData.LicencePlate },
                        { "isOpen", vehicleGateData.IsOpen ? 1f : 0f },
                        { "isEntering", vehicleGateData.IsEntering ? 1f : 0f },
                        { "consumptionPerMinute", vehicleGateData.ConsumptionPerMinute }

                };
                var vehicleGateDataTags = new Dictionary<string, string>
                {
                        { "deviceId", vehicleGate.Id.ToString() }
                };
                vehicleGateService.AddPoint(vehicleGateDataInflux, vehicleGateDataTags);
            }
        }

        public void ChangeMode(VehicleGate vehicle, bool isPublic)
        {
            string action = isPublic ? "public" : "private";
            string payload = JsonConvert.SerializeObject(new { action });

            PublishMessageToSmartDevice(vehicle, payload);
        }
    }
}
