using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.SPU
{
    public class Lamp : SmartDevice
    {
        public Double CurrentBrightness { get; set; }
        public Double BrightnessLimit { get; set; } 
    }
}
