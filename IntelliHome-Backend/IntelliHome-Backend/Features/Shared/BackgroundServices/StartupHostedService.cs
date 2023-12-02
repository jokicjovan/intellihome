using Data.Context;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using IntelliHome_Backend.Features.Users.Services.Interfaces;

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

                Task.Run(() => simulationService.SetupDevicesFromDatabase());
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
    }
}
