using Data.Models.Home;

namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class GetCityDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Country { get; set; }
        public String ZipCode { get; set; }

        public GetCityDTO(City city)
        {
            Id = city.Id;
            Name = city.Name;
            Country = city.Country;
            ZipCode = city.ZipCode;
        }
    }
}
