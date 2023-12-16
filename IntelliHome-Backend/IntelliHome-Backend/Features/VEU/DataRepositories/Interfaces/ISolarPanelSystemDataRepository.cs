using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces
{
    public interface ISolarPanelSystemDataRepository
    {
        void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        void AddActionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
        SolarPanelSystemProductionDataDTO GetLastProductionData(Guid id);
    }
}
