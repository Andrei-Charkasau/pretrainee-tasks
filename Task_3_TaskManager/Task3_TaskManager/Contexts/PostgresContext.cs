using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
