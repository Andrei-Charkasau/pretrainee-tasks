using Microsoft.Data.Sqlite;
using System.Data;

namespace Task3_TaskManager
{
    public class DapperContext : IContext
    {
        private readonly string ConnectionString;

        public DapperContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(connectionString:ConnectionString);
        }
    }
}
