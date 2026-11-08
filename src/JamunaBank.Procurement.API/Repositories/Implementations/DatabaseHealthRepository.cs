using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;

namespace JamunaBank.Procurement.API.Repositories.Implementations;

public sealed class DatabaseHealthRepository(IStoredProcedureExecutor storedProcedures)
    : BaseRepository(storedProcedures), IDatabaseHealthRepository
{
    private const string GetOrganizationUnitsProcedure = "dbo.usp_Organization_GetAll";

    public async Task<int> CountActiveOrganizationUnitsAsync(CancellationToken cancellationToken)
    {
        var parameters = new[]
        {
            SqlParameterFactory.Input("@OrgUnitType", SqlDbType.VarChar, null, 20),
            SqlParameterFactory.Input("@ActiveOnly", SqlDbType.Bit, true)
        };
        var rows = await StoredProcedures.QueryAsync(
            GetOrganizationUnitsProcedure,
            reader => reader.GetInt64(reader.GetOrdinal("ORG_UNIT_ID")),
            parameters,
            cancellationToken);
        return rows.Count;
    }

    public Task<SystemVersionDto?> GetSystemVersionAsync(CancellationToken cancellationToken) =>
        StoredProcedures.QuerySingleOrDefaultAsync(
            "dbo.usp_SystemVersion_Get",
            reader => new SystemVersionDto(
                reader.GetString(reader.GetOrdinal("VERSION_NO")),
                reader.IsDBNull(reader.GetOrdinal("RELEASE_NAME")) ? null : reader.GetString(reader.GetOrdinal("RELEASE_NAME")),
                reader.GetDateTime(reader.GetOrdinal("APPLIED_ON")),
                reader.GetString(reader.GetOrdinal("DATABASE_NAME"))),
            [],
            cancellationToken);
}
