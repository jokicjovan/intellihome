using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces
{
    public interface ISolarPanelSystemDataRepository
    {
        List<SolarPanelSystemDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        SolarPanelSystemDataDTO GetLastData(Guid id);
    }
}
