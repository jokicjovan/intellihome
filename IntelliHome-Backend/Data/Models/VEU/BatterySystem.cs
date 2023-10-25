using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class BatterySystem : SmartDevice
    {
        public List<Battery> Batteries { get; set; }
        public Decimal Capacity => Batteries.Sum(battery => battery.Capacity);
        public Decimal CurrentCapacity => Batteries.Sum(battery => battery.CurrentCapacity);
    }
}
