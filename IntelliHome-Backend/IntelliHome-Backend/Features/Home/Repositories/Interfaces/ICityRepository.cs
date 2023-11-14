using Data.Models.Home;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.Home.Repositories.Interfaces
{
    public interface ICityRepository : ICrudRepository<City>
    {
        Task<City> FindByNameAndCountry(string city, string country);
    }
}
