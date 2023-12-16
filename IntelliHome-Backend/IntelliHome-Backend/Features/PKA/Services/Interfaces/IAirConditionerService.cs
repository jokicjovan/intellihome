using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IAirConditionerService : ICrudService<AirConditioner>
    {
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task AddScheduledWork(string id, double temperature, string mode, string startDate, string endDate);
        Task ChangeMode(Guid id, string mode);
        Task ChangeTemperature(Guid id, double temperature);
        IEnumerable<AirConditioner> GetAllWithHome();
        List<AirConditionerData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        List<AirConditionerData> GetLastHourData(Guid id);
        Task<AirConditionerDTO> GetWithData(Guid id);
        Task<AirConditioner> GetWithHome(Guid id);
        Task ToggleAmbientSensor(Guid id, bool turnOn = true);
    }
}
