using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services.Interfaces
{
    public interface IWashingMachineService : ICrudService<WashingMachine>
    {
        List<WashingMachineMode> GetWashingMachineModes(List<Guid> modesIds);
        Task<IEnumerable<WashingMachineMode>> GetAllWashingMachineModes();
        Task<WashingMachine> Create(WashingMachine entity);
        Task ToggleWashingMachine(Guid id, string username, bool turnOn = true);
        Task<WashingMachineDTO> GetWithData(Guid id);
        Task ChangeMode(Guid id, string mode, string username);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
        List<WashingMachineData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        List<WashingMachineData> GetLastHourData(Guid id);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task AddScheduledWork(string id, double temperature, string mode, string startDate, string endDate, string username);
        IEnumerable<WashingMachine> GetAllWithHome();
        Task<WashingMachine> GetWithHome(Guid id);
    }
}
