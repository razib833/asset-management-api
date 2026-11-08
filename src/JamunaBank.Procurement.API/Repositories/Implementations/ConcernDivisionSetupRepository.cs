using System.Data;using JamunaBank.Procurement.API.Data;using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Repositories.Base;using JamunaBank.Procurement.API.Repositories.Interfaces;using Microsoft.Data.SqlClient;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class ConcernDivisionSetupRepository(IStoredProcedureExecutor p):BaseRepository(p),IConcernDivisionSetupRepository
{
 public Task<IReadOnlyList<ConcernDivisionDto>> GetDivisionsAsync(bool a,CancellationToken c)=>StoredProcedures.QueryAsync("dbo.usp_ConcernDivisionSetup_GetAll",r=>new ConcernDivisionDto(r.GetInt64(r.GetOrdinal("CONCERN_DIVISION_ID")),r.GetInt64(r.GetOrdinal("ORG_UNIT_ID")),r.GetString(r.GetOrdinal("SETUP_CODE")),r.GetBoolean(r.GetOrdinal("IS_ACTIVE")),r.GetString(r.GetOrdinal("ORG_UNIT_CODE")),r.GetString(r.GetOrdinal("ORG_UNIT_NAME")),r.GetString(r.GetOrdinal("ORG_UNIT_TYPE"))),[I("@ActiveOnly",SqlDbType.Bit,a)],c);
 public Task<IReadOnlyList<ConcernOfficialDto>> GetOfficialsAsync(long id,bool a,CancellationToken c)=>StoredProcedures.QueryAsync("dbo.usp_ConcernDivisionOfficial_GetByDivision",MapOfficial,[I("@ConcernDivisionId",SqlDbType.BigInt,id),I("@ActiveOnly",SqlDbType.Bit,a)],c);
 public Task<IReadOnlyList<ConcernAuthorityDto>> GetAuthoritiesAsync(long id,bool a,CancellationToken c)=>StoredProcedures.QueryAsync("dbo.usp_ConcernDivisionAuthority_GetByDivision",MapAuthority,[I("@ConcernDivisionId",SqlDbType.BigInt,id),I("@ActiveOnly",SqlDbType.Bit,a)],c);
 public Task<StoredProcedureResult?> AddOfficialAsync(long id,AddConcernOfficialRequest x,string a,CancellationToken c)=>Run("dbo.usp_ConcernDivisionOfficial_Add",[I("@ConcernDivisionId",SqlDbType.BigInt,id),I("@EmployeeId",SqlDbType.VarChar,x.EmployeeId.Trim(),30),I("@EffectiveFrom",SqlDbType.Date,x.EffectiveFrom),I("@EffectiveTo",SqlDbType.Date,x.EffectiveTo),I("@Actor",SqlDbType.VarChar,a,50)],c);
 public Task<StoredProcedureResult?> DeactivateOfficialAsync(long id,string a,CancellationToken c)=>Run("dbo.usp_ConcernDivisionOfficial_Deactivate",[I("@ConcernOfficialId",SqlDbType.BigInt,id),I("@Actor",SqlDbType.VarChar,a,50)],c);
 public Task<StoredProcedureResult?> AddAuthorityAsync(long id,AddConcernAuthorityRequest x,string a,CancellationToken c)=>Run("dbo.usp_ConcernDivisionAuthority_Add",[I("@ConcernDivisionId",SqlDbType.BigInt,id),I("@EmployeeId",SqlDbType.VarChar,x.EmployeeId.Trim(),30),I("@AuthorityLevel",SqlDbType.Int,x.AuthorityLevel),I("@EffectiveFrom",SqlDbType.Date,x.EffectiveFrom),I("@EffectiveTo",SqlDbType.Date,x.EffectiveTo),I("@Actor",SqlDbType.VarChar,a,50)],c);
 public Task<StoredProcedureResult?> DeactivateAuthorityAsync(long id,string a,CancellationToken c)=>Run("dbo.usp_ConcernDivisionAuthority_Deactivate",[I("@ConcernAuthorityId",SqlDbType.BigInt,id),I("@Actor",SqlDbType.VarChar,a,50)],c);
 private Task<StoredProcedureResult?> Run(string n,IEnumerable<SqlParameter> ps,CancellationToken c)=>StoredProcedures.QuerySingleOrDefaultAsync(n,StoredProcedureResult.FromReader,ps,c);
 private static SqlParameter I(string n,SqlDbType t,object? v,int? s=null)=>SqlParameterFactory.Input(n,t,v,s);
 private static long ToInt64(SqlDataReader r,string name)=>Convert.ToInt64(r[name]);
 private static int ToInt32(SqlDataReader r,string name)=>Convert.ToInt32(r[name]);
 private static bool ToBoolean(SqlDataReader r,string name)=>Convert.ToBoolean(r[name]);
 private static DateOnly ToDateOnly(SqlDataReader r,string name)=>ConvertDate(r[name]);
 private static DateOnly? ToNullableDateOnly(SqlDataReader r,string name){var value=r[name];return value is DBNull?null:ConvertDate(value);}
 private static DateOnly ConvertDate(object value)=>value is DateOnly date?date:DateOnly.FromDateTime(Convert.ToDateTime(value));
 private static ConcernOfficialDto MapOfficial(SqlDataReader r)
 {
  var mappingId=ToInt64(r,"CONCERN_OFFICIAL_ID");var divisionId=ToInt64(r,"CONCERN_DIVISION_ID");var employeeId=r.GetString(r.GetOrdinal("EMPLOYEE_ID"));
  var isActive=ToBoolean(r,"IS_ACTIVE");var effectiveFrom=ToDateOnly(r,"EFFECTIVE_FROM");var effectiveTo=ToNullableDateOnly(r,"EFFECTIVE_TO");
  var fullName=r.GetNullableString("DISPLAY_NAME");var email=r.GetNullableString("EMAIL");var designation=r.GetNullableString("DESIGNATION");
  return new(mappingId,divisionId,employeeId,fullName,email,designation,isActive,effectiveFrom,effectiveTo);
 }
 private static ConcernAuthorityDto MapAuthority(SqlDataReader r)
 {
  var mappingId=ToInt64(r,"CONCERN_AUTHORITY_ID");var divisionId=ToInt64(r,"CONCERN_DIVISION_ID");var employeeId=r.GetString(r.GetOrdinal("EMPLOYEE_ID"));var level=ToInt32(r,"AUTHORITY_LEVEL");
  var isActive=ToBoolean(r,"IS_ACTIVE");var effectiveFrom=ToDateOnly(r,"EFFECTIVE_FROM");var effectiveTo=ToNullableDateOnly(r,"EFFECTIVE_TO");
  var fullName=r.GetNullableString("DISPLAY_NAME");var email=r.GetNullableString("EMAIL");var designation=r.GetNullableString("DESIGNATION");
  return new(mappingId,divisionId,employeeId,fullName,email,designation,level,isActive,effectiveFrom,effectiveTo);
 }
}
