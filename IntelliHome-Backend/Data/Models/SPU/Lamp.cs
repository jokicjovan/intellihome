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
        public Decimal CurrentBrightness { get; set; }
        public Decimal BrightnessLimit { get; set; } 
    }
}
