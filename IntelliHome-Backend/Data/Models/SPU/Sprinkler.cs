using Data.Models.Shared;

namespace Data.Models.SPU
{
    public class Sprinkler : SmartDevice
    {
        public Double PowerPerHour { get; set; }
        public List<SprinklerWork>? ScheduledWorks { get; set; }
    }
}
