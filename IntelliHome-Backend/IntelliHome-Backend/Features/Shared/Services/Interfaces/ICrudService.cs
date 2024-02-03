using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Shared.Services.Interfaces
{
    public interface ICrudService<T> where T : IBaseEntity
    {
        Task<T> Create(T entity);
        Task<T> Get(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Update(T entity);
        Task<T> Delete(Guid id);
    }
}
