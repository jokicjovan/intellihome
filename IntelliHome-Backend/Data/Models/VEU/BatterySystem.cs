using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class BatterySystem : SmartDevice
    {
        public Double Capacity { get; set; }
        public Double CurrentCapacity { get; set; }

        public BatterySystem()
        {

        }
    }
}
