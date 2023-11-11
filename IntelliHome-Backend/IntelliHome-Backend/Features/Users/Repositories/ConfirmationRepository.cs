using Data.Context;
using Data.Models.Users;
using IntelliHome_Backend.Features.Shared.Repositories;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Users.Repositories
{
    public class ConfirmationRepository : CrudRepository<Confirmation>, IConfirmationRepository
    {
        public ConfirmationRepository(PostgreSqlDbContext context) : base(context) { }
        public async Task<Confirmation> FindConfirmationByCode(int code)
        {
            return await _entities.Include(e => e.User).FirstOrDefaultAsync(e => e.Code == code);
        }

        public async Task<Confirmation> FindConfirmationByUserId(Guid id)
        {
            return await _entities.Include(e => e.User).FirstOrDefaultAsync(e => e.User.Id == id);
        }
    }
}
