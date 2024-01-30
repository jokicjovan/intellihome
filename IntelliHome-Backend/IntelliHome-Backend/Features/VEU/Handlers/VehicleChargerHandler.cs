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
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using Newtonsoft.Json;
using IntelliHome_Backend.Features.VEU.DTOs.VehicleCharger;
using Newtonsoft.Json.Serialization;

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

            using var scope = serviceProvider.CreateScope();
            var vehicleChargerService = scope.ServiceProvider.GetRequiredService<IVehicleChargerService>();
            var vehicleCharger = await vehicleChargerService.Get(Guid.Parse(vehicleChargerId));
            if (vehicleCharger == null) {
                return;
            }

            var vehicleChargerData = JsonConvert.DeserializeObject<VehicleChargerDataDTO>(e.ApplicationMessage.ConvertPayloadToString());
            if (vehicleChargerData.BusyChargingPoints != null)
            {
                List<VehicleChargingPointDTO> busyChargingPoints = new List<VehicleChargingPointDTO>();
                foreach (VehicleChargingPointDataDTO chargingPointdataDTO in vehicleChargerData.BusyChargingPoints)
                {
                    var fields = new Dictionary<string, object>
                    {
                        { "currentCapacity", chargingPointdataDTO.CurrentCapacity}
                    };
                    var tags = new Dictionary<string, string>
                    {
                        { "deviceId", chargingPointdataDTO.Id.ToString() }
                    };
                    vehicleChargerService.AddVehicleChargingPointMeasurement(fields, tags);


                    VehicleChargingPoint chargingPoint = vehicleCharger.ChargingPoints.FirstOrDefault(e => e.Id == chargingPointdataDTO.Id);
                    if (chargingPoint == null)
                    {
                        continue;
                    }
                    VehicleChargingPointDTO busyChargingPoint = new VehicleChargingPointDTO
                    {
                        CurrentCapacity = chargingPointdataDTO.CurrentCapacity,
                        Id = chargingPointdataDTO.Id,
                        ChargeLimit = chargingPoint.ChargeLimit,
                        Status = chargingPoint.Status,
                        Capacity = chargingPoint.Capacity,
                        StartTime = chargingPoint.StartTime,
                        EndTime = chargingPoint.EndTime,
                        InitialCapacity = chargingPoint.InitialCapacity
                    };
                    busyChargingPoints.Add(busyChargingPoint);
                }
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };
                var payload = JsonConvert.SerializeObject(new { ConsumptionPerMinute = vehicleChargerData.ConsumptionPerMinute, BusyChargingPoints = busyChargingPoints}, serializerSettings);
                _ = smartDeviceHubContext.Clients.Group(vehicleChargerId).ReceiveSmartDeviceData(payload);
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

                VehicleChargingPoint chargingPoint = vehicleCharger.ChargingPoints.FirstOrDefault(e => e.Id == vehicleChargingPointActionData.ChargingPointId);
                if (chargingPoint == null)
                {
                    return;
                }
                if (vehicleChargingPointActionData.Action == "chargingStarted") {
                    chargingPoint.Status = "CHARGING";
                    chargingPoint.StartTime = DateTime.Now;
                }
                else if (vehicleChargingPointActionData.Action == "chargingFinished") {
                    chargingPoint.EndTime = DateTime.Now;
                }
                vehicleChargerService.Update(chargingPoint);
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
