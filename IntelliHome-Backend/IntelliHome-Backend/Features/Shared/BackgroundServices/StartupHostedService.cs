using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Infrastructure;
using IntelliHome_Backend.Features.Shared.Redis;
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
                RedisRepository<object> redisRepository = scope.ServiceProvider.GetRequiredService<RedisRepository<object>>();

                //eliminsanje lazy loadinga :(
                ISmartHomeHandler smartHomeHandler = scope.ServiceProvider.GetRequiredService<ISmartHomeHandler>();

                Task.Run(() => SetupDevicesFromDatabase());
                Task.Run(() => heartbeatService.SetupLastWillHandler());
                Task.Run(() => redisRepository.DeleteAll());
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
                var connectTasks = smartDevices.Select(async smartDevice =>
                {
                    bool isConnected = false;
                    switch (smartDevice.Type)
                    {
                        case SmartDeviceType.AMBIENTSENSOR:
                            isConnected = await ambientSensorHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.AIRCONDITIONER:
                            isConnected = await airConditionerHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.WASHINGMACHINE:
                            isConnected = await washingMachineHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.LAMP:
                            isConnected = await lampHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.VEHICLEGATE:
                            isConnected = await vehicleGateHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.SPRINKLER:
                            isConnected = await sprinklerHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.BATTERYSYSTEM:
                            isConnected = await batterySystemHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.SOLARPANELSYSTEM:
                            isConnected = await solarPanelSystemHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        case SmartDeviceType.VEHICLECHARGER:
                            isConnected = await vehicleChargerHandler.ConnectToSmartDevice(smartDevice);
                            break;
                        default:
                            Console.WriteLine($"Unsupported SmartDeviceType: {smartDevice.Type}");
                            break;
                    }
                    smartDevice.IsConnected = isConnected;
                }).ToList();
                await Task.WhenAll(connectTasks);
                smartDeviceService.UpdateAll(smartDevices);
                smartDeviceService.UpdateAvailability(smartDevices.Select(s => s.Id).ToList(), true);
            }
        }
    }
}
