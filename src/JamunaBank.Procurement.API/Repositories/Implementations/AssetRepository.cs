using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class AssetRepository(IStoredProcedureExecutor procedures) : BaseRepository(procedures), IAssetRepository
{
    public Task<IReadOnlyList<AssetDto>> GetAllAsync(long? categoryId, bool activeOnly, CancellationToken ct) =>
        StoredProcedures.QueryAsync("dbo.usp_Asset_GetAll", r => Map(r, true),
        [SqlParameterFactory.Input("@CategoryId", SqlDbType.BigInt, categoryId), SqlParameterFactory.Input("@ActiveOnly", SqlDbType.Bit, activeOnly)], ct);

    public Task<AssetDto?> GetByIdAsync(long id, CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_Asset_GetById", r => Map(r, false),
        [SqlParameterFactory.Input("@AssetId", SqlDbType.BigInt, id)], ct);

    public Task<StoredProcedureResult?> CreateAsync(CreateAssetRequest x, string actor, CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_Asset_Create", StoredProcedureResult.FromReader,
        [SqlParameterFactory.Input("@CategoryId",SqlDbType.BigInt,x.AssetCategoryId),SqlParameterFactory.Input("@AssetCode",SqlDbType.VarChar,x.AssetCode.Trim(),30),SqlParameterFactory.Input("@AssetName",SqlDbType.NVarChar,x.AssetName.Trim(),200),SqlParameterFactory.Input("@Description",SqlDbType.NVarChar,x.Description,1000),SqlParameterFactory.Input("@Uom",SqlDbType.NVarChar,x.UnitOfMeasurement.Trim(),50),SqlParameterFactory.Input("@DefaultWarranty",SqlDbType.Int,x.DefaultWarranty),SqlParameterFactory.Input("@UsefulLife",SqlDbType.Int,x.UsefulLife),SqlParameterFactory.Input("@CapitalizationThreshold",SqlDbType.Decimal,x.CapitalizationThreshold),SqlParameterFactory.Input("@Criticality",SqlDbType.VarChar,x.Criticality.Trim().ToUpperInvariant(),20),SqlParameterFactory.Input("@IsInventoryItem",SqlDbType.Bit,x.IsInventoryItem),SqlParameterFactory.Input("@IsDepreciable",SqlDbType.Bit,x.IsDepreciable),SqlParameterFactory.Input("@Actor",SqlDbType.VarChar,actor,50)], ct);

    public Task<StoredProcedureResult?> UpdateAsync(long id, UpdateAssetRequest x, string actor, CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_Asset_Update", StoredProcedureResult.FromReader,
        [SqlParameterFactory.Input("@AssetId",SqlDbType.BigInt,id),SqlParameterFactory.Input("@CategoryId",SqlDbType.BigInt,x.AssetCategoryId),SqlParameterFactory.Input("@AssetCode",SqlDbType.VarChar,x.AssetCode.Trim(),30),SqlParameterFactory.Input("@AssetName",SqlDbType.NVarChar,x.AssetName.Trim(),200),SqlParameterFactory.Input("@Description",SqlDbType.NVarChar,x.Description,1000),SqlParameterFactory.Input("@Uom",SqlDbType.NVarChar,x.UnitOfMeasurement.Trim(),50),SqlParameterFactory.Input("@DefaultWarranty",SqlDbType.Int,x.DefaultWarranty),SqlParameterFactory.Input("@UsefulLife",SqlDbType.Int,x.UsefulLife),SqlParameterFactory.Input("@CapitalizationThreshold",SqlDbType.Decimal,x.CapitalizationThreshold),SqlParameterFactory.Input("@Criticality",SqlDbType.VarChar,x.Criticality.Trim().ToUpperInvariant(),20),SqlParameterFactory.Input("@IsInventoryItem",SqlDbType.Bit,x.IsInventoryItem),SqlParameterFactory.Input("@IsDepreciable",SqlDbType.Bit,x.IsDepreciable),SqlParameterFactory.Input("@IsActive",SqlDbType.Bit,x.IsActive),SqlParameterFactory.Input("@Actor",SqlDbType.VarChar,actor,50)], ct);

    private static AssetDto Map(Microsoft.Data.SqlClient.SqlDataReader r, bool includesCategoryName) => new(
        r.GetInt64(r.GetOrdinal("ASSET_ID")),r.GetInt64(r.GetOrdinal("ASSET_CATEGORY_ID")),includesCategoryName?r.GetNullableString("CATEGORY_NAME"):null,r.GetString(r.GetOrdinal("ASSET_CODE")),r.GetString(r.GetOrdinal("ASSET_NAME")),r.GetNullableString("DESCRIPTION"),r.GetString(r.GetOrdinal("UNIT_OF_MEASUREMENT")),r.GetNullable<int>("DEFAULT_WARRANTY"),r.GetNullable<int>("USEFUL_LIFE"),r.GetNullable<decimal>("CAPITALIZATION_THRESHOLD"),r.GetString(r.GetOrdinal("CRITICALITY")),r.GetBoolean(r.GetOrdinal("IS_INVENTORY_ITEM")),r.GetBoolean(r.GetOrdinal("IS_DEPRECIABLE")),r.GetBoolean(r.GetOrdinal("IS_ACTIVE")));
}
