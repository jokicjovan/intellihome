using Data.Models.Home;
using Data.Models.Shared;
using Data.Models.Users;
using IntelliHome_Backend.Features.Home.DTOs;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
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
            User user = await _userRepository.Read(username) ?? throw new ResourceNotFoundException("User with provided username not found!");
            City city = await _cityRepository.Read(dto.CityId) ?? throw new ResourceNotFoundException("City with provided Id not found!");

            SmartHome smartHome = new SmartHome
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Address = dto.Address,
                City = city,
                Area = dto.Area,
                Type = dto.Type,
                NumberOfFloors = dto.NumberOfFloors,
                Image = dto.Image,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsApproved = false,
                SmartDevices = new List<SmartDevice>(),
                Owner = user
            };

            //TODO: Save image to file system

            
            _smartHomeRepository.Create(smartHome);

            return new GetSmartHomeDTO(smartHome);


        }

        public async Task<List<GetSmartHomeDTO>> GetSmartHomesForUser(string username)
        {
            User user = _userRepository.Read(username).Result ?? throw new ResourceNotFoundException("User with provided username not found!");
            List<SmartHome> smartHomes = await _smartHomeRepository.GetSmartHomesForUser(user);
            return smartHomes.Select(s => new GetSmartHomeDTO(s)).ToList();
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
