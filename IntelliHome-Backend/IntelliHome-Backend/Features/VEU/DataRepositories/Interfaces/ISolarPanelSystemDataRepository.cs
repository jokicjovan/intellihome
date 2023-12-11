using IntelliHome_Backend.Features.VEU.DTOs;

namespace IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces
{
    public interface ISolarPanelSystemDataRepository
    {
        List<SolarPanelSystemProductionDataDTO> GetProductionHistoricalData(Guid id, DateTime from, DateTime to);
        void AddProductionMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags);
        SolarPanelSystemProductionDataDTO GetLastProductionData(Guid id);
    }
}
