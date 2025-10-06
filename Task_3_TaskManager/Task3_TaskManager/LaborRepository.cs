using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    public class LaborRepository : IRepository<Labor>
    {
        const string TableName = "Tasks";
        const int DbDelay = 1000;
        static readonly string CreateQuery = $"INSERT INTO {TableName} (Title, Description, IsCompleted, CreatedAt) VALUES (@title, @description, @isCompleted, @createdAt)";
        static readonly string DeleteQuery = $"DELETE FROM {TableName} WHERE Id = @id";
        static readonly string UpdateStatusQuery = $"UPDATE {TableName} SET IsCompleted = @isCompleted WHERE Id = @id";
        static readonly string GetQuery = $"SELECT Id, Title, Description, IsCompleted, CreatedAt FROM {TableName} WHERE Id = @id";
        static readonly string GetAllQuery = $"SELECT Id, Title, Description, IsCompleted, CreatedAt FROM {TableName}";

        private readonly IContext Context;

        public LaborRepository (IContext dapperContext)
        {
            Context = dapperContext;
        }

        public async Task CreateAsync(Labor labor)
        {
            var query = CreateQuery;
            var parameters = new DynamicParameters();
            parameters.Add("@title", labor.Title);
            parameters.Add("@description", labor.Description);
            parameters.Add("@isCompleted", labor.IsCompleted);
            parameters.Add("@createdAt", labor.CreatedAt);
            using (var connection = Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                await Task.Delay(DbDelay);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var query = DeleteQuery;
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                await Task.Delay(DbDelay);
            }
        }

        public async Task UpdateStatusAsync(int id, bool isCompleted)
        {
            var query = UpdateStatusQuery;
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            parameters.Add("@isCompleted", isCompleted);
            using (var connection = Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                await Task.Delay(DbDelay);
            }
        }

        public async Task<Labor> GetAsync(int id)
        {
            Labor task = null;
            var query = GetQuery;
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = Context.CreateConnection())
            {
                var result = await connection.QueryAsync<Labor>(query, parameters);
                await Task.Delay(DbDelay);
                task = result.FirstOrDefault();
            }
            return task;
        }

        public async Task<List<Labor>> GetAllAsync()
        {
            List<Labor> labors = null;
            var query = GetAllQuery;
            using(var connection = Context.CreateConnection())
            {
                var result = await connection.QueryAsync<Labor>(query);
                await Task.Delay(DbDelay);
                labors = result.ToList();
            }
            return labors;
        }
    }
}
