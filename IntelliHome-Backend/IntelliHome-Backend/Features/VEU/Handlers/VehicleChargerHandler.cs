using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using MQTTnet.Client;
using MQTTnet;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using IntelliHome_Backend.Features.Home.Handlers;
using Data.Models.VEU;
using IntelliHome_Backend.Features.VEU.DTOs.SolarPanelSystem;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;
using IntelliHome_Backend.Features.VEU.Services;

namespace IntelliHome_Backend.Features.VEU.Handlers
{
    public class VehicleChargerHandler : SmartDeviceHandler, IVehicleChargerHandler
    {
        public VehicleChargerHandler(IMqttService mqttService, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttService, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            this.mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.VEU}/{SmartDeviceType.VEHICLECHARGER}/+", HandleMessageFromDevice);
        }

        protected override async Task HandleMessageFromDevice(MqttApplicationMessageReceivedEventArgs e)
        {
            String[] topic_parts = e.ApplicationMessage.Topic.Split('/');
            if (topic_parts.Length < 5)
            {
                Console.WriteLine("Error handling topic");
                return;
            }
            string vehicleChargerId = topic_parts.Last();
            _ = smartDeviceHubContext.Clients.Group(vehicleChargerId).ReceiveSmartDeviceData(e.ApplicationMessage.ConvertPayloadToString());

            using var scope = serviceProvider.CreateScope();
            var vehicleChargerService = scope.ServiceProvider.GetRequiredService<IVehicleChargerService>();
            var vehicleCharger = await vehicleChargerService.Get(Guid.Parse(vehicleChargerId));
            if (vehicleCharger == null) {
                return;
            }

            var vehicleChargerData = JsonConvert.DeserializeObject<VehicleChargerDataDTO>(e.ApplicationMessage.ConvertPayloadToString());
            if (vehicleChargerData.BusyChargingPoints != null)
            {
                foreach (VehicleChargingPointDataDTO chargingPointdataDTO in vehicleChargerData.BusyChargingPoints)
                {

                    var fields = new Dictionary<string, object>
                    {
                        { "startTime", chargingPointdataDTO.StartTime},
                        { "endTime", chargingPointdataDTO.EndTime},
                        { "currentCapacity", chargingPointdataDTO.CurrentCapacity},
                        { "status", chargingPointdataDTO.Status},
                    };
                    var tags = new Dictionary<string, string>
                    {
                        { "deviceId", chargingPointdataDTO.ChargingPointId.ToString() }
                    };
                    vehicleChargerService.AddVehicleChargingPointMeasurement(fields, tags);
                }
                return;
            };

            var vehicleChargingPointActionData = JsonConvert.DeserializeObject<VehicleChargerActionDataDTO>(e.ApplicationMessage.ConvertPayloadToString());
            if (vehicleChargingPointActionData != null)
            {
                var fields = new Dictionary<string, object>
                {
                    { "action", vehicleChargingPointActionData.Action }

                };
                var tags = new Dictionary<string, string>
                {
                    { "actionBy", "SYSTEM"},
                    { "deviceId", vehicleCharger.Id.ToString()}
                };
                vehicleChargerService.AddActionMeasurement(fields, tags);
            }
        }

        public override async Task<bool> ConnectToSmartDevice(SmartDevice smartDevice)
        {
            using var scope = serviceProvider.CreateScope();
            var vehicleChargerService = scope.ServiceProvider.GetRequiredService<IVehicleChargerService>();
            VehicleCharger vehicleCharger = await vehicleChargerService.GetWithHome(smartDevice.Id);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
            {
                { "power_per_hour", vehicleCharger.PowerPerHour },
                { "charging_points_ids", vehicleCharger.ChargingPoints.Select(e => e.Id).ToList()}
            };
            var requestBody = new
            {
                device_id = smartDevice.Id,
                smart_home_id = smartDevice.SmartHome.Id,
                device_category = smartDevice.Category.ToString(),
                device_type = smartDevice.Type.ToString(),
                host = "localhost",
                port = 1883,
                keepalive = 30,
                kwargs = additionalAttributes
            };
            return await simualtionsHandler.AddDeviceToSimulator(requestBody);
        }
    }
}
