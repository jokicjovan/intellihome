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
            City city = await _cityRepository.FindByNameAndCountry(dto.City, dto.Country) ?? throw new ResourceNotFoundException("City is not supported");


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

        public async Task<SmartHomePaginatedDTO> GetSmartHomesForUser(String username, String search, PageParametersDTO pageParameters)
        {
            User user = _userRepository.FindByUsername(username).Result ?? throw new ResourceNotFoundException("User with provided username not found!");
            List<SmartHome> smartHomes = await _smartHomeRepository.GetSmartHomesForUserWithNameSearch(user, search);
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
            _sendApprovalRejectionMail(user, "", false);
        }

        public async Task DeleteSmartHome(Guid id, Guid userId, String reason)
        {
            //TODO: Send mail to user
            User user = await _userRepository.Read(userId) ?? throw new ResourceNotFoundException("User with provided Id not found!");
            _sendApprovalRejectionMail(user, reason, true);
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
    }
}
