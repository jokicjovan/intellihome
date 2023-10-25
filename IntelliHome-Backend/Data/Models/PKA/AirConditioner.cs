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
        public Decimal MinTemperature { get; set; }
        public Decimal MaxTemperature { get; set; }
        public Decimal CurrentTemperature { get; set; }
        public List<AirConditionerMode> Modes { get; set; }
        public AirConditionerMode CurrentMode { get; set; }
    }
}
