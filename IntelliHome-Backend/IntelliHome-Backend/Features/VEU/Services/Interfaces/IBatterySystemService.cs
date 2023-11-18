using Data.Models.VEU;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface IBatterySystemService
    {
        Task<BatterySystem> CreateBatterySystem(BatterySystem batterySystem);
        Task<BatterySystem> GetBatterySystem(Guid Id);
    }
}
