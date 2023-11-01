using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.VEU
{
    public class SolarPanel : IBaseEntity
    {
        public Guid Id { get; set; }
        public Double Area { get; set; }
        public Double Efficiency { get; set; }

        public SolarPanel()
        {

        }
    }
}
