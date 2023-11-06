using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class BatterySystem : SmartDevice
    {
        public List<Battery> Batteries { get; set; }
        public Double Capacity => Batteries.Sum(battery => battery.Capacity);
        public Double CurrentCapacity => Batteries.Sum(battery => battery.CurrentCapacity);

        public BatterySystem()
        {
            Batteries = new List<Battery>();
        }
    }
}
