using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Infrastructure;
using IntelliHome_Backend.Features.Shared.Redis;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Home.Services
{
    public class SmartDeviceService : ISmartDeviceService
    {
        private readonly ISmartDeviceRepository _smartDeviceRepository;
        private readonly ISmartDeviceHandler _smartDeviceHandler;
        private readonly ISmartDeviceDataRepository _smartDeviceDataRepository; 
        private readonly IDataChangeListener _dataChangeListener;

        public SmartDeviceService(ISmartDeviceRepository smartDeviceRepository, ISmartDeviceHandler smartDeviceHandler, ISmartDeviceDataRepository smartDeviceDataRepository, IDataChangeListener dataChangeListener)
        {
            _smartDeviceRepository = smartDeviceRepository;
            _smartDeviceHandler = smartDeviceHandler;
            _smartDeviceDataRepository = smartDeviceDataRepository;
            _dataChangeListener = dataChangeListener;
        }

        public Task<SmartDevice> Create(SmartDevice entity)
        {
            return _smartDeviceRepository.Create(entity);
        }

        public Task<SmartDevice> Delete(Guid id)
        {
            return _smartDeviceRepository.Delete(id);
        }

        public Task<SmartDevice> Get(Guid id)
        {
            return _smartDeviceRepository.Read(id);
        }

        public Task<IEnumerable<SmartDevice>> GetAll()
        {
            return _smartDeviceRepository.ReadAll();
        }

        public IEnumerable<SmartDevice> GetAllWithHome()
        {
            return _smartDeviceRepository.FindAllWIthHome();
        }

        public Task<SmartDevice> Update(SmartDevice entity)
        {
            return _smartDeviceRepository.Update(entity);
        }

        public IEnumerable<SmartDevice> UpdateAll(List<SmartDevice> smartDevices)
        {
            return _smartDeviceRepository.UpdateAll(smartDevices);
        }

        public void UpdateAvailability(List<Guid> smartDevices, Boolean isConnected)
        {
            foreach (var smartDevice in smartDevices)
            {
                var fields = new Dictionary<string, object>
                {
                    { "isConnected", isConnected ? 1 : 0 }

                };
                var tags = new Dictionary<string, string>
                {
                    { "deviceId", smartDevice.ToString()}
                };
                _smartDeviceDataRepository.AddPoint(fields, tags);
            }
            
        }

        public List<AvailabilityData> GetAvailabilityData(Guid id, string h)
        {
            return _smartDeviceDataRepository.GetAvailabilityData(id, h);
        }

        public async Task<(IEnumerable<SmartDeviceDTO>, int)> GetPagedSmartDevicesForSmartHome(Guid smartHomeId, int page, int pageSize)
        {
            string cacheKey = $"SmartDevicesForSmartHome:{smartHomeId}";
            RedisRepository<IEnumerable<SmartDeviceDTO>> redisRepository = new RedisRepository<IEnumerable<SmartDeviceDTO>>("localhost");

            IEnumerable<SmartDeviceDTO>? cachedDtos = redisRepository.Get(cacheKey);
            if (cachedDtos != null)
            {
                Console.WriteLine("Data retrieved from cache.");
                return (cachedDtos, cachedDtos.Count());
            }
            Console.WriteLine("Data retrieved from database.");
            IQueryable<SmartDevice> query = GetSmartDevicesForSmartHome(smartHomeId).AsQueryable();
            int totalItems = await query.CountAsync();
            IEnumerable<SmartDevice> entities = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            IEnumerable<SmartDeviceDTO> dtos = entities.Select(entity => new SmartDeviceDTO(entity));

            redisRepository.Add(cacheKey, dtos);
            return (dtos, totalItems);
        }

        public IEnumerable<SmartDevice> GetSmartDevicesForSmartHome(Guid smartHomeId) {
            return _smartDeviceRepository.FindSmartDevicesForSmartHome(smartHomeId);
        }

        public Task<bool> IsUserAllowed(Guid smartDeviceId, Guid userId) {
            return _smartDeviceRepository.IsUserAllowed(smartDeviceId, userId);
        }

        public async Task TurnOnSmartDevice(Guid id, bool turnOn)
        {
            SmartDevice smartDevice = await _smartDeviceRepository.FindWithSmartHome(id);
            await _smartDeviceHandler.ToggleSmartDevice(smartDevice, turnOn);

            smartDevice.IsOn = turnOn;
            _ = _smartDeviceRepository.Update(smartDevice);

        }
    }
}
