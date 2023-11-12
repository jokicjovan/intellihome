using Data.Models.Users;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;

namespace IntelliHome_Backend.Features.Users.Repositories.Interfaces
{
    public interface IConfirmationRepository : ICrudRepository<Confirmation>
    {
        Task<Confirmation> FindConfirmationByCode(int code);
        Task<Confirmation> FindConfirmationByUserId(Guid id);
    }
}
