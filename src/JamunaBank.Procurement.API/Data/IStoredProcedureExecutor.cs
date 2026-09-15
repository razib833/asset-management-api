using System.Data;
using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Data;

public interface IStoredProcedureExecutor
{
    Task<int> ExecuteNonQueryAsync(string procedureName, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default);
    Task<T?> ExecuteScalarAsync<T>(string procedureName, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default);
    Task<T?> QuerySingleOrDefaultAsync<T>(string procedureName, Func<SqlDataReader, T> map, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> QueryAsync<T>(string procedureName, Func<SqlDataReader, T> map, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default);
    Task<DataSet> QueryDataSetAsync(string procedureName, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default);
}
