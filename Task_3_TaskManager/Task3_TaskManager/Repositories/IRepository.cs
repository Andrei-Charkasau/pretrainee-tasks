using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    public interface IRepository<T>
        where T : class
    {
        Task CreateAsync(T entity);

        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<T> GetAsync(int id);
        Task <List<T>> GetAllAsync();
    }
}
