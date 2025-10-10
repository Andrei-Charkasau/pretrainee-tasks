using System.Data;

namespace Task3_TaskManager
{
    public interface IContext
    {
        IDbConnection CreateConnection();
    }
}
