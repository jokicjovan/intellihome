using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Services.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;

namespace IntelliHome_Backend.Features.Communications.Services
{
    public class SimulationService : ISimulationService
    {
        private readonly IServiceProvider _serviceProvider;
        public SimulationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> ToggleDeviceSimulator(SmartDevice smartDevice, bool turnOn = true)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string baseUrl = "http://127.0.0.1:8000/";
                    string endpoint = turnOn ? "add-device" : "remove-device";
                    string apiUrl = baseUrl + endpoint;

                    var content = new StringContent(string.Empty);
                    HttpResponseMessage response = await client.PostAsync($"{apiUrl}/{smartDevice.Id}", content);

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

        public async Task SetupSimulatorsFromDatabase(bool turnOn = true)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();

                List<SmartDevice> smartDevices = smartDeviceService.GetAllSmartDevices().ToList();
                foreach (SmartDevice smartDevice in smartDevices)
                {
                    bool success = await ToggleDeviceSimulator(smartDevice, turnOn);
                    if (success) smartDevice.IsConnected = turnOn;
                }
                smartDeviceService.UpdateAllSmartDevices(smartDevices);
            }
        }
    }
}
