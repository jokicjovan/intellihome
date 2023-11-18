using Data.Models.Users;

namespace IntelliHome_Backend.Features.Users.Services.Interfaces
{
    public interface IConfirmationService
    {
        Task ActivateAccount(int code);
        Task<Confirmation> CreateActivationConfirmation(Guid userId);
        Task SendActivationMail(User user, int code);
        Task SendPasswordMail(User user, string password);
    }
}
