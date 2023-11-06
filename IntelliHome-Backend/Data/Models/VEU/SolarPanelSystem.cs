using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class SolarPanelSystem : SmartDevice
    {
        public List<SolarPanel> SolarPanels { get; set; }
        public Double Area => SolarPanels.Sum(solarPanel => solarPanel.Area);
        public Double Efficiency => SolarPanels.Sum(solarPanel => solarPanel.Efficiency);

        public SolarPanelSystem()
        {
            SolarPanels = new List<SolarPanel>();
        }
    }
}
