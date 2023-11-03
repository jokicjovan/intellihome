
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
        public String? Image { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public Boolean IsApproved { get; set; }
        [JsonIgnore]
        public List<SmartDevice> SmartDevices { get; set; }
        public User Owner { get; set; }

        public SmartHome(Guid id, string name, string address, City city, string area, SmartHomeType type, short numberOfFloors, string? image, double latitude, double longitude, bool isApproved, User owner)
        {
            Id = id;
            Name = name;
            Address = address;
            City = city;
            Area = area;
            Type = type;
            NumberOfFloors = numberOfFloors;
            Image = image;
            Latitude = latitude;
            Longitude = longitude;
            IsApproved = isApproved;
            SmartDevices = new List<SmartDevice>();
            Owner = owner;
        }

        public SmartHome()
        {
            SmartDevices = new List<SmartDevice>();
        }
    }
}
