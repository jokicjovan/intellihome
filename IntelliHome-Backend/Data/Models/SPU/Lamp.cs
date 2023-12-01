using Data.Models.Shared;

namespace Data.Models.SPU
{
    public class Lamp : SmartDevice
    {
        public Double CurrentBrightness { get; set; }
        public Double BrightnessLimit { get; set; }
        public Double PowerPerHour { get; set; }
    }
}
