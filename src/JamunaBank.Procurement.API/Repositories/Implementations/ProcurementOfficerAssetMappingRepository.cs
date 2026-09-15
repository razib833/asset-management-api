using System.Data;using JamunaBank.Procurement.API.Data;using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Repositories.Base;using JamunaBank.Procurement.API.Repositories.Interfaces;using Microsoft.Data.SqlClient;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class ProcurementOfficerAssetMappingRepository(IStoredProcedureExecutor p):BaseRepository(p),IProcurementOfficerAssetMappingRepository
{
 public Task<IReadOnlyList<ProcurementOfficerAssetMappingDto>> GetByAssetAsync(long id,bool activeOnly,CancellationToken ct)=>StoredProcedures.QueryAsync("dbo.usp_AssetOfficerMap_GetByAsset",Map,[I("@AssetId",SqlDbType.BigInt,id),I("@ActiveOnly",SqlDbType.Bit,activeOnly)],ct);
 public Task<StoredProcedureResult?> AddAsync(long id,AddProcurementOfficerAssetMappingRequest x,string actor,CancellationToken ct)=>Run("dbo.usp_AssetOfficerMap_Save",[I("@AssetId",SqlDbType.BigInt,id),I("@EmployeeId",SqlDbType.VarChar,x.EmployeeId.Trim(),30),I("@IsPrimary",SqlDbType.Bit,x.IsPrimary),I("@EffectiveFrom",SqlDbType.Date,x.EffectiveFrom),I("@EffectiveTo",SqlDbType.Date,x.EffectiveTo),I("@Actor",SqlDbType.VarChar,actor,50)],ct);
 public Task<StoredProcedureResult?> DeactivateAsync(long assetId,long id,string actor,CancellationToken ct)=>Run("dbo.usp_AssetOfficerMap_Deactivate",[I("@AssetId",SqlDbType.BigInt,assetId),I("@MapId",SqlDbType.BigInt,id),I("@Actor",SqlDbType.VarChar,actor,50)],ct);
 private Task<StoredProcedureResult?> Run(string n,IEnumerable<SqlParameter> ps,CancellationToken ct)=>StoredProcedures.QuerySingleOrDefaultAsync(n,StoredProcedureResult.FromReader,ps,ct);private static SqlParameter I(string n,SqlDbType t,object? v,int? s=null)=>SqlParameterFactory.Input(n,t,v,s);
 private static ProcurementOfficerAssetMappingDto Map(SqlDataReader r)
 {
  var mappingId=r.GetInt64(r.GetOrdinal("MAP_ID"));var assetId=r.GetInt64(r.GetOrdinal("ASSET_ID"));var employeeId=r.GetString(r.GetOrdinal("EMPLOYEE_ID"));
  var primary=r.GetBoolean(r.GetOrdinal("IS_PRIMARY"));var from=DateOnly.FromDateTime(r.GetDateTime(r.GetOrdinal("EFFECTIVE_FROM")));DateOnly? to=r.GetNullable<DateTime>("EFFECTIVE_TO") is DateTime d?DateOnly.FromDateTime(d):null;var active=r.GetBoolean(r.GetOrdinal("IS_ACTIVE"));
  var fullName=r.GetNullableString("DISPLAY_NAME");var email=r.GetNullableString("EMAIL");var designation=r.GetNullableString("DESIGNATION");
  return new(mappingId,assetId,employeeId,fullName,email,designation,primary,from,to,active);
 }
}
