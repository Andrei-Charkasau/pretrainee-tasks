using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
