using Data.Models.Home;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;

namespace IntelliHome_Backend.Features.Home.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        public CityService(ICityRepository cityRepository) {
            _cityRepository = cityRepository;
        }

        public Task<City> Create(City entity)
        {
            throw new NotImplementedException();
        }

        public Task<City> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<City>> GetAll()
        {
            return _cityRepository.ReadAll();
        }

        public Task<City> Update(City entity)
        {
            throw new NotImplementedException();
        }

        public Task<City> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
