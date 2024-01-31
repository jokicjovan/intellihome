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
        public VehicleChargerHandler(MqttFactory mqttFactory, IServiceProvider serviceProvider, ISimulationsHandler simualtionsHandler, IHubContext<SmartDeviceHub, ISmartDeviceClient> smartDeviceHubContext)
            : base(mqttFactory, serviceProvider, simualtionsHandler, smartDeviceHubContext)
        {
            mqttService.SubscribeAsync($"FromDevice/+/{SmartDeviceCategory.VEU}/{SmartDeviceType.VEHICLECHARGER}/+", HandleMessageFromDevice);
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

            #region data
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
                    if (chargingPoint != null)
                    {
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
                }

                var payload = JsonConvert.SerializeObject(new { vehicleChargerData.ConsumptionPerMinute, BusyChargingPoints = busyChargingPoints}, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });
                _ = smartDeviceHubContext.Clients.Group(vehicleChargerId).ReceiveSmartDeviceData(payload);
                return;
            };
            #endregion

            #region action
            var vehicleChargingPointActionData = JsonConvert.DeserializeObject<VehicleChargerActionDataDTO>(e.ApplicationMessage.ConvertPayloadToString());
            if (vehicleChargingPointActionData != null)
            {
                VehicleChargingPoint chargingPoint = vehicleCharger.ChargingPoints.FirstOrDefault(e => e.Id == vehicleChargingPointActionData.ChargingPointId);
                if (chargingPoint == null)
                {
                    return;
                }

                String action = "NO ACTION";
                if (vehicleChargingPointActionData.Action == "chargingStarted") {
                    chargingPoint.Status = "CHARGING";
                    chargingPoint.StartTime = DateTime.Now;
                    action = "CHARGING STARTED";
                }
                else if (vehicleChargingPointActionData.Action == "chargingFinished")
                {
                    chargingPoint.Status = "FINISHED";
                    chargingPoint.EndTime = DateTime.Now;
                    action = "CHARGING FINISHED";
                }
                vehicleChargerService.Update(chargingPoint);
                vehicleChargerService.SaveActionAndInformUsers(action, "SYSTEM", vehicleCharger.Id.ToString());
            }
            #endregion
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
