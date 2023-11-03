using Data.Models.Shared;
using IntelliHome_Backend.Features.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Shared.Repositories
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class, IBaseEntity
    {
        protected DbContext _context;
        protected DbSet<T> _entities;

        public CrudRepository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> ReadAll()
        {
            return _entities.ToList();
        }

        public virtual async Task<T> Read(Guid id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public virtual async Task<T> Create(T entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            var entityToUpdate = await Read(entity.Id);
            if (entityToUpdate != null)
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }

            return entityToUpdate;
        }

        public virtual async Task<T> Delete(Guid id)
        {
            var entityToDelete = await Read(id);
            if (entityToDelete != null)
            {
                _context.Remove(entityToDelete);
                _context.SaveChanges();
            }

            return entityToDelete;
        }
    }
}
