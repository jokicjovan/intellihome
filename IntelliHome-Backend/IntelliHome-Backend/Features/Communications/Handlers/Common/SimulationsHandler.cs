using Data.Models.Shared;
using IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IntelliHome_Backend.Features.Communications.Handlers.Common
{
    public class SimulationsHandler : ISimulationsHandler
    {
        private readonly IServiceProvider _serviceProvider;
        public SimulationsHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> AddDeviceToSimulator(SmartDevice smartDevice)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string baseUrl = "http://localhost:8080/";
                    string endpoint = "add-device";
                    string apiUrl = baseUrl + endpoint;

                    var requestBody = new
                    {
                        device_id = smartDevice.Id,
                        smart_home_id = smartDevice.SmartHome.Id,
                        device_category = smartDevice.Category.ToString(),
                        device_type = smartDevice.Type.ToString(),
                        host = "localhost",
                        port = 1883,
                        keepalive = 30
                    };
                    var content = JsonContent.Create(requestBody, MediaTypeHeaderValue.Parse("application/json"), new JsonSerializerOptions());
                    HttpResponseMessage response = await client.PostAsync($"{apiUrl}/", content);

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

        public async Task AddDevicesFromDatabaseToSimulator()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ISmartDeviceService smartDeviceService = scope.ServiceProvider.GetRequiredService<ISmartDeviceService>();

                List<SmartDevice> smartDevices = smartDeviceService.GetAllWithHome().ToList();
                foreach (SmartDevice smartDevice in smartDevices)
                {
                    smartDevice.IsConnected = await AddDeviceToSimulator(smartDevice);
                }
                smartDeviceService.UpdateAll(smartDevices);
            }
        }
    }
}
