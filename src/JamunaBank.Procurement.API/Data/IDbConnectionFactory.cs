using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Data;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}
