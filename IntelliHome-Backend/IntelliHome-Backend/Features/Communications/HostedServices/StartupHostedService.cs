using IntelliHome_Backend.Features.Communications.Services.Interfaces;

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
                IHeartbeatService heartbeatService = scope.ServiceProvider.GetRequiredService<IHeartbeatService>();
                ISimulationService simulationService = scope.ServiceProvider.GetRequiredService<ISimulationService>();
                Task.Run(() => simulationService.AddDevicesFromDatabaseToSimulator());
                Task.Run(() => heartbeatService.SetupLastWillHandler());
                return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISimulationService simulationService = scope.ServiceProvider.GetRequiredService<ISimulationService>();
                //TODO stop all simulations
                return Task.CompletedTask;
            }
        }
    }
}
