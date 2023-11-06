using System.Text.Json.Serialization;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class SolarPanel : IBaseEntity
    {
        public Guid Id { get; set; }
        public Double Area { get; set; }
        public Double Efficiency { get; set; }
        [JsonIgnore]
        public SolarPanelSystem solarPanelSystem { get; set; }

        public SolarPanel()
        {

        }
    }
}
