using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.Home.DataRepository.Interfaces
{
    public interface ISmartHomeDataRepository
    {
        List<SmartHomeUsageDataDTO> GetUsageHistoricalData(Guid id, DateTime from, DateTime to);
        void AddUsageMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        SmartHomeUsageDataDTO GetLastUsageData(Guid id);
    }
}