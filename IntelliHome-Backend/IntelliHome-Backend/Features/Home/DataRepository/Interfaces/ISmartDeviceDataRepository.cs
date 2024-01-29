using IntelliHome_Backend.Features.Home.DTOs;

namespace IntelliHome_Backend.Features.Home.DataRepository.Interfaces
{
    public interface ISmartDeviceDataRepository
    {
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<AvailabilityData> GetAvailabilityData(Guid id, DateTime from, DateTime to);
    }
}
