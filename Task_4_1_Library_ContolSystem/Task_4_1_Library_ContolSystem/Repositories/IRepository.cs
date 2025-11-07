using System.Linq.Expressions;

namespace Task_4_1_Library_ControlSystem.Repositories
{
    public interface IRepository<T,TId>
        where T : class
    {
        Task InsertAsync(T entity);
        Task DeleteAsync(TId id);
        Task UpdateAsync(T patchEntity);
        Task<T> GetAsync(TId id);
        Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
    }
}
