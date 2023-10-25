using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;
using Data.Models.Users;
using Newtonsoft.Json;

namespace Data.Models.Home
{
    public class SmartHome : IBaseEntity
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public City City { get; set; }
        public String Area { get; set; }
        public SmartHomeType Type { get; set; }
        public Int16 NumberOfFloors { get; set; }
        public String Image { get; set; }
        public Decimal Longitude { get; set; }
        public Decimal Latitude { get; set; }
        public Boolean IsApproved { get; set; }
        [JsonIgnore]
        public List<SmartDevice> SmartDevices { get; set; }
        public User Owner { get; set; }

    }
}
