using Data.Models.Home;
using Data.Models.Shared;
using Data.Models.Users;
using IntelliHome_Backend.Features.Users.DTOs;
using Newtonsoft.Json;

namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class GetSmartHomeDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public CityDTO City { get; set; }
        public String Area { get; set; }
        public SmartHomeType Type { get; set; }
        public Int16 NumberOfFloors { get; set; }
        public String? Image { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public Boolean IsApproved { get; set; }
        [JsonIgnore]
        // public List<GetSmartDeviceDTO> SmartDevices { get; set; }
        public GetUserDTO Owner { get; set; }

        // image from file system


        public GetSmartHomeDTO(SmartHome smartHome)
        {
            Id = smartHome.Id;
            Name = smartHome.Name;
            Address = smartHome.Address;
            City = new CityDTO(smartHome.City);
            Area = smartHome.Area;
            Type = smartHome.Type;
            NumberOfFloors = smartHome.NumberOfFloors;
            Image = smartHome.Image;
            Latitude = smartHome.Latitude;
            Longitude = smartHome.Longitude;
            IsApproved = smartHome.IsApproved;
            // SmartDevices = smartHome.SmartDevices;
            Owner = new GetUserDTO(smartHome.Owner);
        }

        public GetSmartHomeDTO()
        { }
    }
}
