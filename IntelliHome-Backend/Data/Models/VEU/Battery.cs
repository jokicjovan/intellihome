using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class Battery : IBaseEntity
    {
        public Guid Id { get; set; }
        public Decimal Capacity { get; set; }
        public Decimal CurrentCapacity { get; set; }
    }
}
