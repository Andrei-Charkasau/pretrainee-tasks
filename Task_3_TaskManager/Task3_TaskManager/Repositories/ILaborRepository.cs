using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    public interface ILaborRepository<Labor>
    {
        Task CreateAsync(Labor entity);

        Task DeleteAsync(int id);
        Task UpdateAsync(Labor entity);
        Task<Labor> GetAsync(int id);
        Task <List<Labor>> GetAllAsync();
    }
}
