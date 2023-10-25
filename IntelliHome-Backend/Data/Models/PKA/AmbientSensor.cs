using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.PKA
{
    public class AmbientSensor : SmartDevice
    {
        public Decimal Temperature { get; set; }
        public Decimal Humidity { get; set; }
    }
}
