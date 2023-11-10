using IntelliHome_Backend.Features.Communications.Services.Interfaces;

namespace IntelliHome_Backend.Features.Communications.HostedServices
{
    public class HeartbeatHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public HeartbeatHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IHeartbeatService heartbeatService = scope.ServiceProvider.GetRequiredService<IHeartbeatService>();
                var setupSimulatorsTask = Task.Run(async () => await heartbeatService.SetupSimulatorsFromDatabase());
                var setupHeartbeatTrackerTask = Task.Run(async () => await heartbeatService.SetupHeartBeatTrackerAsync());
                return Task.WhenAll(setupSimulatorsTask, setupHeartbeatTrackerTask);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IHeartbeatService heartbeatService = scope.ServiceProvider.GetRequiredService<IHeartbeatService>();
                heartbeatService.SetupSimulatorsFromDatabase(false).Wait();
                return Task.CompletedTask;
            }
        }
    }
}
