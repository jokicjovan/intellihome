
using Data.Models.Home;

namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class SmartHomeCreationDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public Guid CityId { get; set; }
        public string Area { get; set; }
        public SmartHomeType Type { get; set; }
        public short NumberOfFloors { get; set; }
        public IFormFile? Image { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
