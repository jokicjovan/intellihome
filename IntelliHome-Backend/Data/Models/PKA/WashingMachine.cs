using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.PKA
{
    public class WashingMachine : SmartDevice
    {
        public List<WashingMachineMode> Modes { get; set; }
        public WashingMachineMode? CurrentMode { get; set; }
        public DateTime StartTime { get; set; }
        public Double PowerPerHour { get; set; }

        public WashingMachine()
        {
            Modes = new List<WashingMachineMode>();
        }

    }
}
