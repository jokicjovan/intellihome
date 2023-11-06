using Data.Models.VEU;

namespace IntelliHome_Backend.Features.VEU.Services.Interfaces
{
    public interface ISolarPanelService
    {
        Task<SolarPanelSystem> CreateSolarPanelSystem(SolarPanelSystem solarPanelSystem);
        Task<SolarPanelSystem> GetSolarPanelSystem(Guid Id);
        Task<SolarPanel> CreateSolarPanel(SolarPanel solarPanel);
    }
}
