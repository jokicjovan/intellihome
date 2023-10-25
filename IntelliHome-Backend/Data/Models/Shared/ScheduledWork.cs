using Data.Models.PKA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Shared
{
    public abstract class ScheduledWork : IBaseEntity
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public List<DaysInWeek> Days { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        
    }
}
