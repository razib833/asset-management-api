using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;

namespace JamunaBank.Procurement.API.Repositories.Implementations;

public sealed class CommonLookupRepository(IStoredProcedureExecutor storedProcedures)
    : BaseRepository(storedProcedures), ICommonLookupRepository
{
    private const string GetLookupProcedure = "dbo.usp_Common_GetLookup";

    public Task<IReadOnlyList<CommonLookupDto>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken)
    {
        var parameters = new[] { SqlParameterFactory.Input("@LookupCategory", SqlDbType.VarChar, category, 50) };
        return StoredProcedures.QueryAsync(GetLookupProcedure, reader => new CommonLookupDto(
            reader.GetInt64(reader.GetOrdinal("LOOKUP_ID")),
            reader.GetString(reader.GetOrdinal("LOOKUP_CODE")),
            reader.GetString(reader.GetOrdinal("LOOKUP_NAME")),
            reader.GetNullableString("DESCRIPTION"),
            reader.GetInt32(reader.GetOrdinal("DISPLAY_ORDER"))), parameters, cancellationToken);
    }
}
