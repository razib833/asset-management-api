using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Data;

public static class SqlDataReaderExtensions
{
    public static T? GetNullable<T>(this SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? default : reader.GetFieldValue<T>(ordinal);
    }

    public static string? GetNullableString(this SqlDataReader reader, string columnName) =>
        reader.GetNullable<string>(columnName);
}
