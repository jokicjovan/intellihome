using Data.Models.Home;
using Data.Models.Shared;
using Data.Models.Users;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.Home.Services
{
    public class SmartHomeService : ISmartHomeService
    {
        private readonly ISmartHomeRepository _smartHomeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICityRepository _cityRepository;

        public SmartHomeService(ISmartHomeRepository smartHomeRepository, IUserRepository userRepository, ICityRepository cityRepository)
        {
            _smartHomeRepository = smartHomeRepository;
            _userRepository = userRepository;
            _cityRepository = cityRepository;
        }

        public async Task<SmartHome> GetSmartHome(Guid Id)
        {
            SmartHome smartHome = await _smartHomeRepository.Read(Id) ?? throw new ResourceNotFoundException("Smart house with provided Id not found!");
            return smartHome;
        }

        public async Task<GetSmartHomeDTO> GetSmartHomeDTO(Guid Id)
        {
            SmartHome smartHome = await _smartHomeRepository.Read(Id) ?? throw new ResourceNotFoundException("Smart house with provided Id not found!");
            return new GetSmartHomeDTO(smartHome);
        }

        public async Task<GetSmartHomeDTO> CreateSmartHome(SmartHomeCreationDTO dto, String username)
        {
            User user = await _userRepository.FindByUsername(username) ?? throw new ResourceNotFoundException("User with provided username not found!");
            City city = await _cityRepository.Read(dto.CityId) ?? throw new ResourceNotFoundException("City with provided Id not found!");


            //TODO: Save image to file system
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

            return new GetSmartHomeDTO(smartHome);


        }

        public async Task<SmartHomePaginatedDTO> GetSmartHomesForUser(string username, PageParametersDTO pageParameters)
        {
            User user = _userRepository.FindByUsername(username).Result ?? throw new ResourceNotFoundException("User with provided username not found!");
            List<SmartHome> smartHomes = await _smartHomeRepository.GetSmartHomesForUser(user);
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
        }

        public async Task DeleteSmartHome(Guid id)
        {
            await _smartHomeRepository.Delete(id);
        }
    }
}
