using Data.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.PKA
{
    public class WashingMachineWork: ScheduledWork
    {
        public WashingMachineMode Mode { get; set; }
        public Double Temperature { get; set; }
    }
}
