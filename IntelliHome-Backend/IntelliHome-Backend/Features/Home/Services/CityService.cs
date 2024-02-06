using Data.Models.Home;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using System.Linq;
using System.Text.RegularExpressions;

namespace IntelliHome_Backend.Features.Home.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ISmartHomeDataRepository _smartHomeDataRepository;
        private readonly ISmartHomeRepository _smartHomeRepository;

        public CityService(ICityRepository cityRepository, ISmartHomeDataRepository smartHomeDataRepository, ISmartHomeRepository smartHomeRepository)
        {
            _cityRepository = cityRepository;
            _smartHomeDataRepository = smartHomeDataRepository;
            _smartHomeRepository = smartHomeRepository;
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

        public async Task<CityPaginatedDTO> GetAllPaged(String search, PageParametersDTO pageParameters)
        {
            List<City> cities = await _cityRepository.GetCitiesWithNameSearch(search);
            CityPaginatedDTO result = new CityPaginatedDTO
            {
                TotalCount = cities.Count,
                Cities = cities.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                    .Take(pageParameters.PageSize).Select(s => new CityDTO(s)).ToList()
            };
            return result;
        }
        public async Task<List<SmartHomeUsageDataDTO>> GetUsageHistoricalData(Guid id, DateTime from, DateTime to)
        {
            List<SmartHome> smartHomes = await _smartHomeRepository.GetSmartHomesByCity(id);
            List<SmartHomeUsageDataDTO> cityUsage = new List<SmartHomeUsageDataDTO>();

            foreach (SmartHome smartHome in smartHomes)
            {
                cityUsage.AddRange(_smartHomeDataRepository.GetUsageHistoricalData(smartHome.Id, from, to));
            }

            List<SmartHomeUsageDataDTO> cityUsageAggregated =
                cityUsage.GroupBy(e => new DateTime(e.Timestamp.Value.Year, e.Timestamp.Value.Month, e.Timestamp.Value.Day, e.Timestamp.Value.Hour, e.Timestamp.Value.Minute, 0))
                         .Select(group => new SmartHomeUsageDataDTO
                         {
                             Timestamp = group.Key,
                             ConsumptionPerMinute = group.Sum(e => e.ConsumptionPerMinute),
                             ProductionPerMinute = group.Sum(e => e.ProductionPerMinute),
                             GridPerMinute = group.Sum(e => e.GridPerMinute)
                         })
                         .OrderBy(e => e.Timestamp).ToList();

            return cityUsageAggregated;
        }

    }
}
