using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class SolarPanelSystem : SmartDevice
    {
        public Double Area { get; set; }
        public Double Efficiency { get; set; }
        public SolarPanelSystem()
        {
        }
    }
}
