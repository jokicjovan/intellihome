using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.Home
{
    public class City : IBaseEntity
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Country { get; set; }
        public String ZipCode { get; set; }
    }
}
