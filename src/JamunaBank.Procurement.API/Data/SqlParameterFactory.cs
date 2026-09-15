using System.Data;
using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Data;

public static class SqlParameterFactory
{
    public static SqlParameter Input(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = Create(name, type, size);
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }

    public static SqlParameter Output(string name, SqlDbType type, int? size = null)
    {
        var parameter = Create(name, type, size);
        parameter.Direction = ParameterDirection.Output;
        return parameter;
    }

    public static SqlParameter InputOutput(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = Input(name, type, value, size);
        parameter.Direction = ParameterDirection.InputOutput;
        return parameter;
    }

    public static SqlParameter ReturnValue(string name = "@ReturnValue") =>
        new(name, SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

    private static SqlParameter Create(string name, SqlDbType type, int? size)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
    }
}
