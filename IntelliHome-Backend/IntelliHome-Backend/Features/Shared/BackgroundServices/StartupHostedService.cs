using Data.Models.Shared;
using Data.Models.SPU;
using Data.Models.VEU;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;

namespace IntelliHome_Backend.Features.Shared.BackgroundServices
{
    public class StartupHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public StartupHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ILastWillHandler heartbeatService = scope.ServiceProvider.GetRequiredService<ILastWillHandler>();
                ISimulationsHandler simulationService = scope.ServiceProvider.GetRequiredService<ISimulationsHandler>();

                Task.Run(() => SetupDevicesFromDatabase());
                Task.Run(() => heartbeatService.SetupLastWillHandler());
                return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISimulationsHandler simulationService = scope.ServiceProvider.GetRequiredService<ISimulationsHandler>();
                //TODO stop all simulations
                return Task.CompletedTask;
            }
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
                IVehicleGateHandler vehicleGateHandler = scope.ServiceProvider.GetRequiredService<IVehicleGateHandler>();
                ISprinklerHandler sprinklerHandler = scope.ServiceProvider.GetRequiredService<ISprinklerHandler>();
                IBatterySystemHandler batterySystemHandler = scope.ServiceProvider.GetRequiredService<IBatterySystemHandler>();
                ISolarPanelSystemHandler solarPanelSystemHandler = scope.ServiceProvider.GetRequiredService<ISolarPanelSystemHandler>();
                IVehicleChargerHandler vehicleChargerHandler = scope.ServiceProvider.GetRequiredService<IVehicleChargerHandler>();

                List<SmartDevice> smartDevices = smartDeviceService.GetAllWithHome().ToList();
                foreach (SmartDevice smartDevice in smartDevices)
                {
                    if (smartDevice.Type == SmartDeviceType.AMBIENTSENSOR)
                    {
                        smartDevice.IsConnected = await ambientSensorHandler.ConnectToSmartDevice(smartDevice, new Dictionary<string, object>());
                    }
                    else if (smartDevice.Type == SmartDeviceType.AIRCONDITIONER)
                    {
                        smartDevice.IsConnected = await airConditionerHandler.ConnectToSmartDevice(smartDevice, new Dictionary<string, object>());
                    }
                    else if (smartDevice.Type == SmartDeviceType.WASHINGMACHINE)
                    {
                        smartDevice.IsConnected = await washingMachineHandler.ConnectToSmartDevice(smartDevice, new Dictionary<string, object>());
                    }
                    else if (smartDevice.Type == SmartDeviceType.LAMP)
                    {
                        Lamp lamp = (Lamp)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "brightness_limit", lamp.BrightnessLimit },
                        };
                        smartDevice.IsConnected = await lampHandler.ConnectToSmartDevice(smartDevice, additionalAttributes);
                    }
                    else if (smartDevice.Type == SmartDeviceType.VEHICLEGATE)
                    {
                        VehicleGate vehicleGate = (VehicleGate)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "is_public", vehicleGate.IsPublic },
                            { "allowed_licence_plates", vehicleGate.AllowedLicencePlates }
                        };
                        smartDevice.IsConnected = await vehicleGateHandler.ConnectToSmartDevice(smartDevice, additionalAttributes);
                    }
                    else if (smartDevice.Type == SmartDeviceType.SPRINKLER)
                    {
                        smartDevice.IsConnected = await sprinklerHandler.ConnectToSmartDevice(smartDevice, new Dictionary<string, object>());
                    }
                    else if (smartDevice.Type == SmartDeviceType.BATTERYSYSTEM)
                    {
                        smartDevice.IsConnected = await batterySystemHandler.ConnectToSmartDevice(smartDevice, new Dictionary<string, object>());
                    }
                    else if (smartDevice.Type == SmartDeviceType.SOLARPANELSYSTEM)
                    {
                        SolarPanelSystem solarPanelSystem = (SolarPanelSystem)smartDevice;
                        Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
                        {
                            { "area", solarPanelSystem.Area },
                            { "efficiency", solarPanelSystem.Efficiency }
                        };
                        smartDevice.IsConnected = await solarPanelSystemHandler.ConnectToSmartDevice(smartDevice, additionalAttributes);
                    }
                    else if (smartDevice.Type == SmartDeviceType.VEHICLECHARGER)
                    {
                        smartDevice.IsConnected = await vehicleChargerHandler.ConnectToSmartDevice(smartDevice, new Dictionary<string, object>());
                    }
                }
                smartDeviceService.UpdateAll(smartDevices);
            }
        }
    }
}
