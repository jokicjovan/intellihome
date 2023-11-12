using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Home;
using Data.Models.Shared;

namespace Data.Models.Users
{
    public class Admin : User
    {
        public Boolean IsSuperAdmin { get; set; }

        public Admin(Guid id,string firstName, string lastName, string email, string username, string password, bool isActivated, string? image,bool isSuperAdmin)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            Password = password;
            IsActivated = isActivated;
            Image = image;
            SmartHomes = new List<SmartHome>();
            AllowedSmartDevices = new List<SmartDevice>();
            IsSuperAdmin = isSuperAdmin;
        }
    }
}
