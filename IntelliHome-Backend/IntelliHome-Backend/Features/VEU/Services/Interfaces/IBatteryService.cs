using Data.Models.VEU;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IBatteryService
    {
        public Task<BatterySystem> CreateBatterySystem(BatterySystem batterySystem);
        public Task<Battery> AddBatteryToSystem(Guid batterySystemId, Battery battery);
        public Task<BatterySystem> GetBatterySystem(Guid Id);
    }
}
