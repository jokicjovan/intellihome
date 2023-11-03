using Data.Models.Shared;
using Data.Models.Users;

namespace Data.Models.Home
{
    public class SmartHomeApproveRequest : IBaseEntity
    {
        public Guid Id { get; set; }
        public SmartHome SmartHome { get; set; }
        public User User { get; set; }
        public Boolean IsApproved { get; set; }
        public String Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
