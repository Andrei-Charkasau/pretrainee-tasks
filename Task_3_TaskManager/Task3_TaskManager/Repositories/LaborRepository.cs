using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    public class LaborRepository : IRepository<Labor>
    {
        const string TableName = "Tasks";
        static readonly string CreateQuery = $"INSERT INTO {TableName} (Title, Description, IsCompleted, CreatedAt) VALUES (@title, @description, @isCompleted, @createdAt)";
        static readonly string DeleteQuery = $"DELETE FROM {TableName} WHERE Id = @id";
        static readonly string UpdateStatusQuery = $"UPDATE {TableName} SET Title = @title, Description = @description, IsCompleted = @isCompleted WHERE Id = @id";
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
            }
        }

        public async Task UpdateAsync(Labor labor)
        {
            var query = UpdateStatusQuery;
            var parameters = new DynamicParameters();
            parameters.Add("@id", labor.Id);
            parameters.Add("@title", labor.Title);
            parameters.Add("@description", labor.Description);
            parameters.Add("@isCompleted", labor.IsCompleted);
            using (var connection = Context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<Labor> GetAsync(int id)
        {
            Labor labor = null;
            var query = GetQuery;
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = Context.CreateConnection())
            {
                labor = await connection.QueryFirstAsync<Labor>(query, parameters);
            }
            return labor;
        }

        public async Task<List<Labor>> GetAllAsync()
        {
            List<Labor> labors = null;
            var query = GetAllQuery;
            using(var connection = Context.CreateConnection())
            {
                var result = await connection.QueryAsync<Labor>(query);
                labors = result.ToList();
            }
            return labors;
        }
    }
}
