using Data.Models.Shared;

namespace Data.Models.PKA
{
    public class WashingMachine : SmartDevice
    {
        public List<WashingMachineMode> Modes { get; set; }
        public WashingMachineMode? Mode { get; set; }

        public List<WashingMachineWork>? ScheduledWorks { get; set; }
        public DateTime StartTime { get; set; }
        public Double PowerPerHour { get; set; }

        public WashingMachine()
        {
            Modes = new List<WashingMachineMode>();
            ScheduledWorks = new List<WashingMachineWork>();
        }

    }
}
