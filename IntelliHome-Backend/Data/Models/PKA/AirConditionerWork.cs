using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.PKA
{
    public class AirConditionerWork : ScheduledWork
    {
        public AirConditionerMode Mode { get; set; }
        public Decimal Temperature { get; set; }

    }
}
