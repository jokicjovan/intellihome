using System.Text.Json.Serialization;
using Data.Models.Home;
using Data.Models.Users;

namespace Data.Models.Shared
{
    public class SmartDevice : IBaseEntity
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String? Image { get; set; }
        public SmartDeviceCategory Category { get; set; }
        public SmartDeviceType Type { get; set; }
        public Boolean IsConnected { get; set; }
        public Boolean IsOn { get; set; }
        [JsonIgnore]
        public SmartHome SmartHome { get; set; }
        [JsonIgnore]
        public List<User> AllowedUsers { get; set; }

    }
}
