using System.Data;
using JamunaBank.Procurement.API.Data;
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
}
