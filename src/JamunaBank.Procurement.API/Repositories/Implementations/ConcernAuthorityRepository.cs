using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class ConcernAuthorityRepository(IStoredProcedureExecutor p):BaseRepository(p),IConcernAuthorityRepository
{
 public Task<IReadOnlyList<ConcernAuthorityPendingDto>> GetPendingAsync(string e,CancellationToken c)=>StoredProcedures.QueryAsync("dbo.usp_ConcernAuthority_GetPending",r=>new ConcernAuthorityPendingDto(r.GetInt64(r.GetOrdinal("REQUISITION_ID")),r.GetString(r.GetOrdinal("REQUISITION_NO")),r.GetDateTime(r.GetOrdinal("REQUESTED_DATE")),r.GetDecimal(r.GetOrdinal("TOTAL_ESTIMATED_AMOUNT")),r.GetInt64(r.GetOrdinal("CONCERN_DIVISION_ID")),r.GetString(r.GetOrdinal("CONCERN_DIVISION_NAME")),r.GetString(r.GetOrdinal("REQUESTED_BY_NAME")),r.GetInt32(r.GetOrdinal("ITEM_COUNT"))),[I("@EmployeeId",SqlDbType.VarChar,e,30)],c);
 public async Task<RequisitionDetailDto?> GetDetailAsync(long id,string e,CancellationToken c)=>RequisitionDetailDataSetMapper.Map(await StoredProcedures.QueryDataSetAsync("dbo.usp_ConcernAuthority_GetDetail",[I("@RequisitionId",SqlDbType.BigInt,id),I("@EmployeeId",SqlDbType.VarChar,e,30)],c));
 public Task<ConcernAuthorityReviewContextDto?> GetContextAsync(long id,string e,CancellationToken c)=>StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_ConcernAuthority_GetReviewContext",r=>new ConcernAuthorityReviewContextDto(r.GetInt64(r.GetOrdinal("WORKFLOW_RULE_ID")),r.GetString(r.GetOrdinal("RULE_CODE")),r.GetString(r.GetOrdinal("RULE_NAME")),r.GetString(r.GetOrdinal("CURRENT_STAGE")),r.GetString(r.GetOrdinal("NEXT_DESTINATION")),r.GetInt64(r.GetOrdinal("CONCERN_DIVISION_ID")),r.GetString(r.GetOrdinal("CONCERN_DIVISION_NAME")),r.GetString(r.GetOrdinal("CONCERN_OFFICIAL_EMPLOYEE_ID")),r.GetNullableString("CONCERN_OFFICIAL_NAME"),r.GetNullableString("CONCERN_OFFICIAL_REMARKS"),r.GetDateTime(r.GetOrdinal("FORWARDED_DATE"))),[I("@RequisitionId",SqlDbType.BigInt,id),I("@EmployeeId",SqlDbType.VarChar,e,30)],c);
 public Task<StoredProcedureResult?> ApproveAsync(long id,string e,string?m,Guid?b,CancellationToken c)=>Run("dbo.usp_ConcernAuthority_Approve",id,e,m,b,c);
 public Task<StoredProcedureResult?> ReturnAsync(long id,string e,string m,CancellationToken c)=>Run("dbo.usp_ConcernAuthority_Return",id,e,m,null,c);
 public Task<StoredProcedureResult?> RejectAsync(long id,string e,string m,Guid?b,CancellationToken c)=>Run("dbo.usp_ConcernAuthority_Reject",id,e,m,b,c);
 private Task<StoredProcedureResult?> Run(string n,long id,string e,string?m,Guid?b,CancellationToken c)=>StoredProcedures.QuerySingleOrDefaultAsync(n,StoredProcedureResult.FromReader,[I("@RequisitionId",SqlDbType.BigInt,id),I("@ActorEmployeeId",SqlDbType.VarChar,e,30),I("@Remarks",SqlDbType.NVarChar,m,2000),I("@BatchReferenceId",SqlDbType.UniqueIdentifier,b)],c);
 private static SqlParameter I(string n,SqlDbType t,object?v,int?s=null)=>SqlParameterFactory.Input(n,t,v,s);
}
