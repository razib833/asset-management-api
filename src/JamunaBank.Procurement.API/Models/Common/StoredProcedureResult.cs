using JamunaBank.Procurement.API.Data;
using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Models;

public sealed record StoredProcedureResult(
    string ResultCode,
    string ResultMessage,
    long? EntityId,
    string? ReferenceNo)
{
    public static StoredProcedureResult FromReader(SqlDataReader reader) => new(
        reader.GetString(reader.GetOrdinal("RESULT_CODE")),
        reader.GetString(reader.GetOrdinal("RESULT_MESSAGE")),
        reader.GetNullable<long>("ENTITY_ID"),
        reader.GetNullableString("REFERENCE_NO"));
}
