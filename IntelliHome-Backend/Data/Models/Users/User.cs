using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Home;
using Data.Models.Shared;

namespace Data.Models.Users
{
    public class User : BaseUser
    {
        public List<SmartHome> SmartHomes { get; set; }

    }
}
