using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Home;
using Data.Models.Shared;

namespace Data.Models.Users
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Image { get; set; }
        public List<SmartHome> SmartHomes { get; set; }

    }
}
