using System.Text.Json.Serialization;
using Data.Models.Home;
using Data.Models.Shared;

namespace Data.Models.Users
{
    public class User : BaseUser
    {

        [JsonIgnore]
        public List<SmartHome> SmartHomes { get; set; }

        public List<SmartDevice> AllowedSmartDevices { get; set; }


        public User( string firstName, string lastName, string email, string username, string password, bool isActivated, string? image)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            Password = password;
            IsActivated = isActivated;
            Image = image;
            SmartHomes = new List<SmartHome>();
            AllowedSmartDevices = new List<SmartDevice>();
        }

        public User()
        {
            SmartHomes = new List<SmartHome>();
            AllowedSmartDevices = new List<SmartDevice>();
        }
    }
}
