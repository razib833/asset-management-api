using JamunaBank.Procurement.API.Configuration;
using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Data;

public sealed class SqlConnectionFactory(DatabaseOptions options) : IDbConnectionFactory
{
    public SqlConnection CreateConnection()
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{DatabaseOptions.ConnectionStringName}' is not configured.");
        }

        // SqlClient enables connection pooling by default for identical connection strings.
        return new SqlConnection(options.ConnectionString);
    }
}
