using Npgsql;
using System.Data;

namespace Task3_TaskManager
{
    public class PostgresContext : IContext
    {
        private readonly string ConnectionString;
        public PostgresContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}
