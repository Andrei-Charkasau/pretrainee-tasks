using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3_TaskManager
{
    public interface IContext
    {
        IDbConnection CreateConnection();
    }
}
