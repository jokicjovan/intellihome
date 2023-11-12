using Data.Models.Users;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.Users.Repositories;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Security.Cryptography;

namespace IntelliHome_Backend.Features.Users.Services
{
    public class ConfirmationService:IConfirmationService
    {
        private const int VerificationCodeLength = 8;
        private readonly IConfirmationRepository _confirmationRepository;
        private readonly IUserRepository _userRepository;

        public ConfirmationService(IUserRepository userRepository, IConfirmationRepository confirmationRepository)
        {
            _userRepository = userRepository;
            _confirmationRepository = confirmationRepository;
        }
        public async Task ActivateAccount(int code)
        {
            Confirmation confirmation = await _confirmationRepository.FindConfirmationByCode(code);
            if (confirmation == null)
            {
                throw new ResourceNotFoundException("Activation code invalid!");
            }

            if (confirmation.ExpirationDate.CompareTo(DateTime.UtcNow) <= 0)
            {
                _confirmationRepository.Delete(confirmation.Id);
                _userRepository.Delete(confirmation.User.Id);
                throw new ResourceNotFoundException("Activation code expired, please sign up again!");
            }

            confirmation.User.IsActivated = true;
            _userRepository.Update(confirmation.User);
            _confirmationRepository.Delete(confirmation.Id);
        }
        public async Task<Confirmation> CreateActivationConfirmation(Guid userId)
        {
            User user = await _userRepository.Read(userId);
            if (user == null)
            {
                throw new ResourceNotFoundException("User not found!");
            }

            Confirmation confirmation = new Confirmation();
            confirmation.Code = await GenerateVerificationCode(VerificationCodeLength);
            confirmation.ExpirationDate = DateTime.Now.AddHours(24);
            confirmation.User = user;
            return await _confirmationRepository.Create(confirmation);
        }

        public async Task SendActivationMail(User user, int code)
        {
            StreamReader sr = new StreamReader("sendgrid_api_key.txt");
            String sendgridApiKey = sr.ReadLine();
            SendGridClient client = new SendGridClient(sendgridApiKey);
            SendGridMessage msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("certificateswebapp@gmail.com", "IntelliHome"));
            msg.AddTo(new EmailAddress(user.Email, String.Concat(user.FirstName, " ", user.LastName)));
            msg.Subject = "Activation mail";
            msg.SetTemplateId("d-59fba3b9758a4695b72111f1041fb95f");

            var dynamicTemplateData = new
            {
                url = "http://localhost:8000/successfulActivation?code=" + code,
                user_name = user.FirstName,
                code = code
                
            };

            msg.SetTemplateData(dynamicTemplateData);
            Response response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Mail error");
            }
        }

        private async Task<int> GenerateVerificationCode(int codeLength)
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(codeLength);
            int verificationCode = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % (int)Math.Pow(10, codeLength);
            if (await _confirmationRepository.FindConfirmationByCode(verificationCode) != null)
            {
                return await GenerateVerificationCode(codeLength);
            }
            return verificationCode;
        }
    }
}
