using Data.Models.PKA;
using Data.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.SPU
{
    public class SprinklerWork : ScheduledWork
    {
        public Double PowerPerHour { get; set; }
    }
    
}
