using Data.Models.PKA;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;

namespace IntelliHome_Backend.Features.SPU.Services.Interfaces
{
    public interface IVehicleGateService : ICrudService<VehicleGate>
    {
        List<VehicleGateData> GetHistoricalData(Guid id, DateTime from, DateTime to);
        Task<VehicleGateDTO> GetWithData(Guid id);
        Task ChangeMode(Guid id, bool isPublic, string username);
        Task ToggleVehicleGate(Guid id, bool turnOn, string username);
        void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags);
        void SaveAction(Dictionary<string, object> fields, Dictionary<string, string> tags);
        Task AddLicencePlate(Guid id, string licencePlate);
        Task RemoveLicencePlate(Guid id, string licencePlate);
        Task OpenCloseGate(Guid id, bool isOpen, string username);
        List<VehicleGateActionData> GetHistoricalActionData(Guid id, DateTime from, DateTime to);
    }
}
