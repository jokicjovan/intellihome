using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
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

        public async Task<bool> TurnOnDeviceSimulator(Guid deviceId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string baseUrl = "http://localhost:8080/";
                    string endpoint = "turn-on-device";
                    string apiUrl = baseUrl + endpoint + "/" + deviceId.ToString();

                    HttpResponseMessage response = await client.PostAsync($"{apiUrl}", null);

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

                    smartDevice.IsConnected = await AddDeviceToSimulator(requestBody);
                }
                smartDeviceService.UpdateAll(smartDevices);
            }
        }
    }
}
