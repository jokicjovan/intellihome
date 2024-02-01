using Data.Models.Home;
using Data.Models.Shared;
using Data.Models.Users;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.Infrastructure;
using IntelliHome_Backend.Features.Shared.Redis;

namespace IntelliHome_Backend.Features.Home.Services
{
    public class SmartHomeService : ISmartHomeService
    {
        private readonly ISmartHomeRepository _smartHomeRepository;
        private readonly ISmartHomeDataRepository _smartHomeDataRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ISmartHomeHandler _smartHomeHandler;
        private readonly ISmartDeviceRepository _smartDeviceRepository;
        private readonly IDataChangeListener _dataChangeListener;

        public SmartHomeService(ISmartHomeRepository smartHomeRepository, IUserRepository userRepository, ICityRepository cityRepository,
            ISmartHomeDataRepository smartHomeDataRepository, ISmartHomeHandler smartHomeHandler, IDataChangeListener dataChangeListener, ISmartDeviceRepository smartDeviceRepository)
        {
            _smartHomeRepository = smartHomeRepository;
            _smartHomeDataRepository = smartHomeDataRepository;
            _userRepository = userRepository;
            _cityRepository = cityRepository;
            _smartHomeHandler = smartHomeHandler;
            _smartDeviceRepository = smartDeviceRepository;
            _dataChangeListener = dataChangeListener;
        }

        // public async Task<GetSmartHomeDTO> GetSmartHomeDTO(Guid Id)
        // {
        //     SmartHome smartHome = await _smartHomeRepository.Read(Id) ?? throw new ResourceNotFoundException("Smart house with provided Id not found!");
        //     return new GetSmartHomeDTO(smartHome);
        // }

        public async Task<GetSmartHomeDTO> GetSmartHomeDTOO(Guid Id)
        { 
            string cacheKey = $"SmartHomeDTO:{Id}";
            RedisRepository<GetSmartHomeDTO> redisRepository = new RedisRepository<GetSmartHomeDTO>("localhost");
            GetSmartHomeDTO smartHomeDTO = redisRepository.Get(cacheKey);
            if (smartHomeDTO != null) return smartHomeDTO;
            SmartHome smartHome = await _smartHomeRepository.Read(Id) ?? throw new ResourceNotFoundException("Smart house with provided Id not found!");
            smartHomeDTO = new GetSmartHomeDTO(smartHome);
            redisRepository.Add(cacheKey, smartHomeDTO);
            return smartHomeDTO;

        }

        public async Task<GetSmartHomeDTO> CreateSmartHome(SmartHomeCreationDTO dto, String username)
        {
            User user = await _userRepository.FindByUsername(username) ?? throw new ResourceNotFoundException("User with provided username not found!");
            City city = await _cityRepository.FindByNameAndCountry(dto.City, dto.Country) ?? throw new ResourceNotFoundException("City is not supported");


            //TODO: Save image to file system
            if(dto.Image == null) throw new InvalidInputException("Image is required!");
            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
            string SavePath = Path.Combine("static/smartHomes", ImageName);
            using (var stream = new FileStream(SavePath, FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }

            SmartHome smartHome = new SmartHome
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Address = dto.Address,
                City = city,
                Area = dto.Area,
                Type = dto.Type,
                NumberOfFloors = dto.NumberOfFloors,
                Image = SavePath,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsApproved = false,
                SmartDevices = new List<SmartDevice>(),
                Owner = user
            };

            _smartHomeRepository.Create(smartHome);
            _dataChangeListener.RegisterListener(DeleteSmartHomeDevicesCache, smartHome.Id);
            _dataChangeListener.HandleDataChange(user.Id);
            return new GetSmartHomeDTO(smartHome);
        }

        public void DeleteSmartHomeDevicesCache(Guid smartHomeId)
        {
            string cacheKey = $"SmartDevicesForSmartHome:{smartHomeId}";
            RedisRepository<IEnumerable<SmartDeviceDTO>> redisRepository = new RedisRepository<IEnumerable<SmartDeviceDTO>>("localhost");
            redisRepository.Delete(cacheKey);
        }

        public void DeleteUserSmartHomesCache(Guid userId)
        {
            string cacheKey = $"SmartHomesForUsername:{userId}";
            RedisRepository<IEnumerable<SmartDeviceDTO>> redisRepository = new RedisRepository<IEnumerable<SmartDeviceDTO>>("localhost");
            redisRepository.Delete(cacheKey);
        }

        // public async Task<SmartHomePaginatedDTO> GetSmartHomesForUser(String username, String search, PageParametersDTO pageParameters)
        // {
        //     User user = _userRepository.FindByUsername(username).Result ?? throw new ResourceNotFoundException("User with provided username not found!");
        //     List<SmartHome> smartHomes = await _smartHomeRepository.GetSmartHomesForUserWithNameSearch(user, search);
        //     // for all smartHomes register listener for data change
        //     foreach (var smartHome in smartHomes)
        //     {
        //         _dataChangeListener.RegisterListener(DeleteSmartHomeDevicesCache, smartHome.Id);
        //     }
        //     _dataChangeListener.RegisterListener(DeleteUserSmartHomesCache, user.Id);
        //     SmartHomePaginatedDTO result = new SmartHomePaginatedDTO
        //     {
        //         TotalCount = smartHomes.Count,
        //         SmartHomes = smartHomes.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
        //             .Take(pageParameters.PageSize).Select(s => new GetSmartHomeDTO(s)).ToList()
        //     };
        //     return result;
        // }

        public async Task<SmartHomePaginatedDTO> GetSmartHomesForUser(String username, String search, PageParametersDTO pageParameters)
        {
            User user = _userRepository.FindByUsername(username).Result ?? throw new ResourceNotFoundException("User with provided username not found!");
            string cacheKey = $"SmartHomesForUsername:{user.Id}";
            RedisRepository<List<SmartHome>> redisRepository = new RedisRepository<List<SmartHome>>("localhost");
            List<SmartHome> smartHomes = redisRepository.Get(cacheKey);
            if (smartHomes == null)
            {
                Console.WriteLine("Data retrieved from database.");
                smartHomes = await _smartHomeRepository.GetSmartHomesForUser(user);
                redisRepository.Add(cacheKey, smartHomes);
            }
            else
            {
                Console.WriteLine("Data retrieved from cache.");
            }
            foreach (var smartHome in smartHomes)
            {
                _dataChangeListener.RegisterListener(DeleteSmartHomeDevicesCache, smartHome.Id);
            }
            _dataChangeListener.RegisterListener(DeleteUserSmartHomesCache, user.Id);
            if (search != null)
            {
                smartHomes = smartHomes.Where(s => s.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            SmartHomePaginatedDTO result = new SmartHomePaginatedDTO
            {
                TotalCount = smartHomes.Count,
                SmartHomes = smartHomes.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                    .Take(pageParameters.PageSize).Select(s => new GetSmartHomeDTO(s)).ToList()
            };
            return result;
        }

        public async Task<SmartHomePaginatedDTO> GetAllPaged(String search, PageParametersDTO pageParameters)
        {
            List<SmartHome> smartHomes = await _smartHomeRepository.GetAllSmartHomesPaged(search);
            SmartHomePaginatedDTO result = new SmartHomePaginatedDTO
            {
                TotalCount = smartHomes.Count,
                SmartHomes = smartHomes.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                    .Take(pageParameters.PageSize).Select(s => new GetSmartHomeDTO(s)).ToList()
            };
            return result;
        }

        public async Task ApproveSmartHome(Guid id)
        {
            SmartHome smartHome = _smartHomeRepository.Read(id).Result ?? throw new ResourceNotFoundException("Smart house with provided Id not found!");
            if(smartHome.IsApproved) throw new InvalidInputException("Smart house is already approved!");
            smartHome.IsApproved = true;
            await _smartHomeRepository.Update(smartHome);


            // TODO: Send mail to user
            User user = await _userRepository.Read(smartHome.Owner.Id) ?? throw new ResourceNotFoundException("User with provided Id not found!");
            _dataChangeListener.HandleDataChange(user.Id);
            _sendApprovalRejectionMail(user, "", false);
        }

        public async Task DeleteSmartHome(Guid id, Guid userId, String reason)
        {
            //TODO: Send mail to user
            User user = await _userRepository.Read(userId) ?? throw new ResourceNotFoundException("User with provided Id not found!");
            _sendApprovalRejectionMail(user, reason, true);
            _dataChangeListener.HandleDataChange(user.Id);
            await _smartHomeRepository.Delete(id);
        }


        private async Task _sendApprovalRejectionMail(User user, String reason, Boolean isRejection)
        {
            StreamReader sr = new StreamReader("sendgrid_api_key.txt");
            String sendgridApiKey = sr.ReadLine();
            SendGridClient client = new SendGridClient(sendgridApiKey);
            SendGridMessage msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("certificateswebapp@gmail.com", "IntelliHome"));
            msg.AddTo(new EmailAddress(user.Email, String.Concat(user.FirstName, " ", user.LastName)));
            msg.Subject = isRejection ? "Smart home rejection mail" : "Smart home approval mail " ;

            string messageContent = isRejection
                ? $"Dear {user.FirstName}, unfortunately, your smart home application has been rejected. Reason: {reason}"
                : $"Dear {user.FirstName}, congratulations! Your smart home application has been approved.";

            msg.AddContent(MimeType.Text, messageContent);
            msg.AddContent(MimeType.Html, $"<p>{messageContent}</p>");

            Response response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Mail error");
            }
        }

        public async Task<SmartHomePaginatedDTO> GetSmartHomesForApproval(PageParametersDTO pageParameters)
        {
            List<SmartHome> smartHomes = await _smartHomeRepository.GetSmartHomesForApproval();
            SmartHomePaginatedDTO result = new SmartHomePaginatedDTO
            {
                TotalCount = smartHomes.Count,
                SmartHomes = smartHomes.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                    .Take(pageParameters.PageSize).Select(s => new GetSmartHomeDTO(s)).ToList()
            };
            return result;
        }

        public async Task<List<String>> GetAllEmailsWithPermission(SmartHome smarthome)
        {
            List<User> users = new List<User>();
            for (int i = 0; i < smarthome.SmartDevices.Count; i++)
            {
                SmartDevice device =await  _smartDeviceRepository.Read(smarthome.SmartDevices[i].Id);
                if (i == 0)
                {
                    users = device.AllowedUsers;
                }
                else
                {
                    users = users.Intersect(device.AllowedUsers).ToList();
                }
            }
            return users.Select(user=>user.Email).ToList();
        }

        public async Task AddPermision(SmartHome smartHome, string email)
        {
            User user =_userRepository.FindByEmail(email).Result ?? throw new ResourceNotFoundException("User with provided username not found!");
            foreach (SmartDevice device in smartHome.SmartDevices)
            {
                if (!user.AllowedSmartDevices.Contains(device))
                {
                    user.AllowedSmartDevices.Add(device);
                }
            }
            await _userRepository.Update(user);


        }

        public async Task RemovePermision(SmartHome smartHome, string email)
        {
            User user = _userRepository.FindByEmail(email).Result ?? throw new ResourceNotFoundException("User with provided username not found!");
            foreach (SmartDevice device in smartHome.SmartDevices)
            {
                if (user.AllowedSmartDevices.Contains(device))
                {
                    user.AllowedSmartDevices.Remove(device);
                }
            }
                await _userRepository.Update(user);
        }

        public Task<SmartHome> Create(SmartHome entity)
        {
            throw new NotImplementedException();
        }

        public async Task<SmartHome> Get(Guid id)
        {
            SmartHome smartHome = await _smartHomeRepository.Read(id) ?? throw new ResourceNotFoundException("Smart house with provided Id not found!");
            return smartHome;
        }

        public Task<IEnumerable<SmartHome>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SmartHome> Update(SmartHome entity)
        {
            throw new NotImplementedException();
        }

        public Task<SmartHome> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserAllowed(Guid smartHomeId, Guid userId) {
            return _smartHomeRepository.IsUserAllowed(smartHomeId, userId);
        }

        public List<SmartHomeUsageDataDTO> GetUsageHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _smartHomeDataRepository.GetUsageHistoricalData(id, from, to);
        }

        public void AddUsageMeasurement(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _smartHomeDataRepository.AddUsageMeasurement(fields, tags);
        }
    }
}
