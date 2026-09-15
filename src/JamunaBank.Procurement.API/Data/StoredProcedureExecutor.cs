using System.Data;
using System.Globalization;
using JamunaBank.Procurement.API.Configuration;
using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Data;

public sealed class StoredProcedureExecutor(IDbConnectionFactory connectionFactory, DatabaseOptions options)
    : IStoredProcedureExecutor
{
    public async Task<int> ExecuteNonQueryAsync(string procedureName, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, procedureName, parameters);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<T?> ExecuteScalarAsync<T>(string procedureName, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, procedureName, parameters);
        var value = await command.ExecuteScalarAsync(cancellationToken);
        return ConvertValue<T>(value);
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string procedureName, Func<SqlDataReader, T> map, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(map);
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, procedureName, parameters);
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? map(reader) : default;
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string procedureName, Func<SqlDataReader, T> map, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(map);
        var records = new List<T>();
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, procedureName, parameters);
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken);
        while (await reader.ReadAsync(cancellationToken)) records.Add(map(reader));
        return records;
    }

    public async Task<DataSet> QueryDataSetAsync(string procedureName, IEnumerable<SqlParameter>? parameters = null, CancellationToken cancellationToken = default)
    {
        var dataSet = new DataSet { Locale = CultureInfo.InvariantCulture };
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, procedureName, parameters);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var resultIndex = 0;
        do
        {
            var table = new DataTable($"Result{resultIndex++}") { Locale = CultureInfo.InvariantCulture };
            for (var columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++)
            {
                table.Columns.Add(reader.GetName(columnIndex), reader.GetFieldType(columnIndex));
            }
            while (await reader.ReadAsync(cancellationToken))
            {
                var values = new object[reader.FieldCount];
                reader.GetValues(values);
                table.Rows.Add(values);
            }
            dataSet.Tables.Add(table);
        } while (await reader.NextResultAsync(cancellationToken));

        return dataSet;
    }

    private SqlCommand CreateCommand(SqlConnection connection, string procedureName, IEnumerable<SqlParameter>? parameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(procedureName);
        var command = connection.CreateCommand();
        command.CommandText = procedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandTimeout = options.CommandTimeoutSeconds;
        if (parameters is not null) command.Parameters.AddRange(parameters.ToArray());
        return command;
    }

    private static T? ConvertValue<T>(object? value)
    {
        if (value is null or DBNull) return default;
        if (value is T typed) return typed;
        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        return (T?)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
    }
}
