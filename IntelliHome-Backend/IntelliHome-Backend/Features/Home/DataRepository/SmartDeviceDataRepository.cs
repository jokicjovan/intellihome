using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Shared.Influx;

namespace IntelliHome_Backend.Features.Home.DataRepository
{
    public class SmartDeviceDataRepository : ISmartDeviceDataRepository
    {
        private readonly InfluxRepository _context;

        public SmartDeviceDataRepository(InfluxRepository context)
        {
            _context = context;
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("availability", fields, tags);
        }
    }
}
