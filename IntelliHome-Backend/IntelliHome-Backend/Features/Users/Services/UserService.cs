using Data.Models.Users;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using BCrypt.Net;

namespace IntelliHome_Backend.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfirmationRepository _confirmationRepository;
        private readonly IConfirmationService _confirmationService;

        public UserService(IUserRepository userRepository,IConfirmationRepository confirmationRepository, IConfirmationService confirmationService)
        {
            _userRepository = userRepository;
            _confirmationRepository = confirmationRepository;
            _confirmationService = confirmationService;
        }

        public async Task<User> CreateUser(User newUser,IFormFile image)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(newUser, new ValidationContext(newUser, null, null), validationResults, true);
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    throw new InvalidInputException(validationResult.ErrorMessage);
                }
            }

            User potentialUser = await _userRepository.FindByEmail(newUser.Email);
            if (potentialUser != null)
            {
                Confirmation potentialUserConfirmation = await _confirmationRepository.FindConfirmationByUserId(potentialUser.Id);
                if (potentialUserConfirmation != null && potentialUserConfirmation.ExpirationDate.CompareTo(DateTime.UtcNow) <= 0)
                {
                    _confirmationRepository.Delete(potentialUserConfirmation.Id);
                    _userRepository.Delete(potentialUserConfirmation.User.Id);
                }
                else
                {
                    throw new InvalidInputException("User with that email already exists!");
                }
            }

            potentialUser =await _userRepository.FindByUsername(newUser.Username);
            if (potentialUser != null)
            {
                Confirmation potentialUserConfirmation = await _confirmationRepository.FindConfirmationByUserId(potentialUser.Id);
                if (potentialUserConfirmation != null && potentialUserConfirmation.ExpirationDate.CompareTo(DateTime.UtcNow) <= 0)
                {
                    _confirmationRepository.Delete(potentialUserConfirmation.Id);
                    _userRepository.Delete(potentialUserConfirmation.User.Id);
                }
                else
                {
                    throw new InvalidInputException("User with that telephone already exists!");
                }
            }
            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string SavePath = Path.Combine("static/profilePictures", ImageName);
            using (var stream = new FileStream(SavePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            newUser.Image = SavePath;
            newUser.Password=BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.IsActivated = false;
            User user = await _userRepository.Create(newUser);
            Confirmation confirmation = await _confirmationService.CreateActivationConfirmation(user.Id);

            try
            {
                _confirmationService.SendActivationMail(user, confirmation.Code);
                return user;
            }
            catch (Exception)
            {
                _userRepository.Delete(user.Id);
                _confirmationRepository.Delete(confirmation.Id);
                throw new InvalidInputException("An error with SMS or Mail service has occured!");
            }
        }
        public async Task<User> Authenticate(String username, String password)
        {
            User user = await _userRepository.FindByUsername(username);
            if (user==null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new ResourceNotFoundException("Email or password is incorrect!");
            }
            else if (!user.IsActivated)
            {
                throw new InvalidInputException("User is not activated!");
            }
            return user;
        }
    }
}
