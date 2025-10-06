using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    public interface IRepository<T>
        where T : class
    {
        Task CreateAsync(T labor);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(int id, bool isCompleted);
        Task<T> GetAsync(int id);
        Task <List<T>> GetAllAsync();
    }
}
