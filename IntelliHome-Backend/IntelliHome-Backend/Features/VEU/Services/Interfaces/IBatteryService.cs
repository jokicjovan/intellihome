using Data.Models.VEU;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IBatteryService
    {
        Task<BatterySystem> CreateBatterySystem(BatterySystem batterySystem);
        Task<BatterySystem> GetBatterySystem(Guid Id);
        Task<Battery> CreateBattery(Battery battery);
    }
}
