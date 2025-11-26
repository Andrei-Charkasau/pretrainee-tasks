using InnoShop.Core.Models;
using InnoShop.Core.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoShop.Core.Repositories.Repositories
{
    public class EntityRepository<T, TId> : IRepository<T, TId>
        where T : BaseEntity<TId>
    {
        private readonly  DbSet<T> _entities;
        private readonly ShopContext _context;

        public EntityRepository(ShopContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task DeleteAsync(TId id)
        {
            var entityToDelete = _entities.FirstOrDefault(e => e.Id.Equals(id));

            if (entityToDelete != null)
            {
                _entities.Remove(entityToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<T> GetAsync(TId id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public IQueryable<T> GetAll()
        {
            return _entities;
        }

        public async Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_entities.Where(predicate));
        }

        public async Task InsertAsync(T entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T patchEntity)
        {
            _entities.Update(patchEntity);
            await _context.SaveChangesAsync();
        }
    }
}
