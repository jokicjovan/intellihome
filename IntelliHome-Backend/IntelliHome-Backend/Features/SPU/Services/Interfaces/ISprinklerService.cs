using Data.Models.PKA;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.Services.Interfaces
{
    public interface ISprinklerService : ICrudService<Sprinkler>
    {
        Task<Sprinkler> GetWithSmartHome(Guid id);
        List<SprinklerData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        Task AddScheduledWork(string scheduleId, bool scheduleIsSpraying, string scheduleStartDate, string? scheduleEndDate, string username);
        Task ToggleSprinkler(Guid id, string username, bool turnSprayingOn);
        Task<SprinklerDTO> GetWithData(Guid id);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task ToggleSprinklerSpraying(Guid id, string username, bool turnOn);
        List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to);
    }
}
