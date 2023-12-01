using Data.Context;
using IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces;
using IntelliHome_Backend.Features.Communications.Handlers.PKA.Interfaces;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using IntelliHome_Backend.Features.Users.Services.Interfaces;

namespace IntelliHome_Backend.Features.Communications.HostedServices
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
                IAmbientSensorHandler ambientSensorHandler = scope.ServiceProvider.GetRequiredService<IAmbientSensorHandler>();

                Task.Run(() => simulationService.AddDevicesFromDatabaseToSimulator());
                Task.Run(() => heartbeatService.SetupLastWillHandler());
                Task.Run(() => ambientSensorHandler.RegisterAmbientSensorListeners());
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
    }
}
