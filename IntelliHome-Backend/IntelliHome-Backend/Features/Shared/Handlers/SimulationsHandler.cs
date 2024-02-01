using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IntelliHome_Backend.Features.Shared.Handlers
{
    public class SimulationsHandler : ISimulationsHandler
    {
        private readonly String _simulatorUrl;
        public SimulationsHandler(IConfiguration configuration)
        {
            _simulatorUrl = configuration["Simulator:Url"];
        }

        public async Task<bool> AddDeviceToSimulator(object deviceRequestBody)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = _simulatorUrl + "/add-device";
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
    }
}
