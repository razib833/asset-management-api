using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class AssetCategoryRepository(IStoredProcedureExecutor procedures) : BaseRepository(procedures), IAssetCategoryRepository
{
    public Task<IReadOnlyList<AssetCategoryDto>> GetAllAsync(bool activeOnly, CancellationToken cancellationToken) =>
        StoredProcedures.QueryAsync("dbo.usp_AssetCategory_GetAll", r => new AssetCategoryDto(
            r.GetInt64(r.GetOrdinal("ASSET_CATEGORY_ID")), r.GetString(r.GetOrdinal("CATEGORY_CODE")),
            r.GetString(r.GetOrdinal("CATEGORY_NAME")), r.GetNullableString("DESCRIPTION"),
            r.GetBoolean(r.GetOrdinal("IS_ACTIVE"))),
            [SqlParameterFactory.Input("@ActiveOnly", SqlDbType.Bit, activeOnly)], cancellationToken);
}
