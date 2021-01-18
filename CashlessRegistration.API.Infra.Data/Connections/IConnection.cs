using System.Data;

namespace CashlessRegistration.API.Infra.Data.Connections
{
    public interface IConnection
    {
        IDbConnection CreateConnection();
    }
}
