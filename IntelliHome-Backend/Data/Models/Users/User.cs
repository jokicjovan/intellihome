using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Home;
using Data.Models.Shared;
using Newtonsoft.Json;

namespace Data.Models.Users
{
    public class User : BaseUser
    {
        [JsonIgnore]
        public List<SmartHome> SmartHomes { get; set; }

        [JsonIgnore]
        public List<SmartDevice> AllowedSmartDevices { get; set; }

    }
}
