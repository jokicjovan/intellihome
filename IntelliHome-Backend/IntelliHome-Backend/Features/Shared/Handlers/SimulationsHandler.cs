using Data.Models.PKA;
using Data.Models.Shared;
using Data.Models.SPU;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IntelliHome_Backend.Features.Shared.Handlers
{
    public class SimulationsHandler : ISimulationsHandler
    {
        private readonly IServiceProvider _serviceProvider;
        public SimulationsHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> AddDeviceToSimulator(object deviceRequestBody)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string baseUrl = "http://localhost:8080/";
                    string endpoint = "add-device";
                    string apiUrl = baseUrl + endpoint;
                    var content = JsonContent.Create(deviceRequestBody, MediaTypeHeaderValue.Parse("application/json"), new JsonSerializerOptions());
                    HttpResponseMessage response = await client.PostAsync($"{apiUrl}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            };
        }

        public async Task SetupDevicesFromDatabase()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();
                IAmbientSensorHandler ambientSensorHandler = scope.ServiceProvider.GetRequiredService<IAmbientSensorHandler>();
                IAirConditionerHandler airConditionerHandler = scope.ServiceProvider.GetRequiredService<IAirConditionerHandler>();
                IWashingMachineHandler washingMachineHandler = scope.ServiceProvider.GetRequiredService<IWashingMachineHandler>();
                ILampHandler lampHandler = scope.ServiceProvider.GetRequiredService<ILampHandler>();
                IVehicleGate vehicleGateHandler = scope.ServiceProvider.GetRequiredService<IVehicleGate>();
                ISprinklerHandler sprinklerHandler = scope.ServiceProvider.GetRequiredService<ISprinklerHandler>();
                IBatterySystemHandler batterySystemHandler = scope.ServiceProvider.GetRequiredService<IBatterySystemHandler>();
                ISolarPanelSystemHandler solarPanelSystemHandler  = scope.ServiceProvider.GetRequiredService<ISolarPanelSystemHandler>();
                IVehicleChargerHandler vehicleChargerHandler = scope.ServiceProvider.GetRequiredService<IVehicleChargerHandler>();

                List<SmartDevice> smartDevices = smartDeviceService.GetAllWithHome().ToList();
                foreach (SmartDevice smartDevice in smartDevices)
                {
                    if (smartDevice.Type == SmartDeviceType.AMBIENTSENSOR)
                    {
                        smartDevice.IsConnected = await ambientSensorHandler.AddSmartDeviceToSimulator(smartDevice, new Dictionary<string, object>());
                        if (smartDevice.IsConnected) ambientSensorHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.AIRCONDITIONER) 
                    {
                        AirConditioner airConditioner = (AirConditioner)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "current_mode", airConditioner.CurrentMode },
                            { "current_temp", airConditioner.CurrentTemperature }
                        };
                        smartDevice.IsConnected = await airConditionerHandler.AddSmartDeviceToSimulator(smartDevice, additionalAttributes);
                        if (smartDevice.IsConnected) airConditionerHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.WASHINGMACHINE)
                    {
                        WashingMachine washingMachine = (WashingMachine)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "mode_name", washingMachine.CurrentMode.Name },
                            { "mode_duration", washingMachine.CurrentMode.Duration },
                            { "mode_temp", washingMachine.CurrentMode.Temperature }
                        };
                        smartDevice.IsConnected = await washingMachineHandler.AddSmartDeviceToSimulator(smartDevice, additionalAttributes);
                        if (smartDevice.IsConnected) washingMachineHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.LAMP)
                    {
                        Lamp lamp = (Lamp)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "brightness_limit", lamp.BrightnessLimit },
                        };
                        smartDevice.IsConnected = await lampHandler.AddSmartDeviceToSimulator(smartDevice, additionalAttributes);
                        if (smartDevice.IsConnected) lampHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.VEHICLEGATE)
                    {
                        VehicleGate vehicleGate = (VehicleGate)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "is_public", vehicleGate.IsPublic },
                            { "allowed_licence_plates", vehicleGate.AllowedLicencePlates }
                        };
                        smartDevice.IsConnected = await vehicleGateHandler.AddSmartDeviceToSimulator(smartDevice, additionalAttributes);
                        if (smartDevice.IsConnected) vehicleGateHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.SPRINKLER)
                    {
                        smartDevice.IsConnected = await sprinklerHandler.AddSmartDeviceToSimulator(smartDevice, new Dictionary<string, object>());
                        if (smartDevice.IsConnected) sprinklerHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.BATTERYSYSTEM)
                    {
                        smartDevice.IsConnected = await batterySystemHandler.AddSmartDeviceToSimulator(smartDevice, new Dictionary<string, object>());
                        if (smartDevice.IsConnected) batterySystemHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.SOLARPANELSYSTEM)
                    {
                        SolarPanelSystem solarPanelSystem = (SolarPanelSystem)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "area", solarPanelSystem.Area },
                            { "efficiency", solarPanelSystem.Efficiency }
                        };
                        smartDevice.IsConnected = await solarPanelSystemHandler.AddSmartDeviceToSimulator(smartDevice, additionalAttributes);
                        if (smartDevice.IsConnected) solarPanelSystemHandler.SubscribeToSmartDevice(smartDevice);
                    }
                    else if (smartDevice.Type == SmartDeviceType.VEHICLECHARGER)
                    {
                        smartDevice.IsConnected = await vehicleChargerHandler.AddSmartDeviceToSimulator(smartDevice, new Dictionary<string, object>());
                        if (smartDevice.IsConnected) vehicleChargerHandler.SubscribeToSmartDevice(smartDevice);
                    }
                }
                smartDeviceService.UpdateAll(smartDevices);
            }
        }
    }
}
