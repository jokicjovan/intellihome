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
        public Double Temperature { get; set; }
        public Double Humidity { get; set; }
        public Double PowerPerHour { get; set; }

        public AmbientSensor()
        {
        }
    }
}
