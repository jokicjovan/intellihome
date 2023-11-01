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
        public Double Capacity { get; set; }
        public Double CurrentCapacity { get; set; }

        public Battery() { }
    }
}
