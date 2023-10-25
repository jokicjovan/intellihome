using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;
using Data.Models.Users;
using NodaTime;

namespace Data.Models.Home
{
    public class SmartHomeApproveRequest : IBaseEntity
    {
        public Guid Id { get; set; }
        public SmartHome SmartHome { get; set; }
        public User User { get; set; }
        public Boolean IsApproved { get; set; }
        public String Reason { get; set; }
        public Instant CreatedAt { get; set; }
    }
}
