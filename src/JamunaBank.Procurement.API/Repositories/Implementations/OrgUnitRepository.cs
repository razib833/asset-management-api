using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;

namespace JamunaBank.Procurement.API.Repositories.Implementations;

public sealed class OrgUnitRepository(IStoredProcedureExecutor storedProcedures)
    : BaseRepository(storedProcedures), IOrgUnitRepository
{
    private const string GetAllProcedure = "dbo.usp_Organization_GetAll";

    public Task<IReadOnlyList<OrgUnitDto>> GetAllAsync(
        string? type,
        bool activeOnly,
        CancellationToken cancellationToken)
    {
        var parameters = new[]
        {
            SqlParameterFactory.Input("@OrgUnitType", SqlDbType.VarChar, type, 20),
            SqlParameterFactory.Input("@ActiveOnly", SqlDbType.Bit, activeOnly)
        };
        return StoredProcedures.QueryAsync(GetAllProcedure, reader => new OrgUnitDto(
            reader.GetInt64(reader.GetOrdinal("ORG_UNIT_ID")),
            reader.GetNullable<long>("PARENT_ORG_UNIT_ID"),
            reader.GetString(reader.GetOrdinal("ORG_UNIT_CODE")),
            reader.GetString(reader.GetOrdinal("ORG_UNIT_NAME")),
            reader.GetString(reader.GetOrdinal("ORG_UNIT_TYPE")),
            reader.GetBoolean(reader.GetOrdinal("IS_ACTIVE"))), parameters, cancellationToken);
    }
}
