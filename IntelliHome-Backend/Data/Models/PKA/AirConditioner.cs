using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.PKA
{
    public class AirConditioner : SmartDevice
    {
        public Double MinTemperature { get; set; }
        public Double MaxTemperature { get; set; }
        public Double CurrentTemperature { get; set; }
        public List<AirConditionerMode> Modes { get; set; }
        public List<AirConditionerWork> ScheduledWorks { get; set; }
        public AirConditionerMode CurrentMode { get; set; }
    }
}
