using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.Users
{
    public class Admin : User
    {
        public Boolean IsSuperAdmin { get; set; }
    }
}
