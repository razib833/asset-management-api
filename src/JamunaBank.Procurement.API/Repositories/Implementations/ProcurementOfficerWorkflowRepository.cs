using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class ProcurementOfficerWorkflowRepository(IStoredProcedureExecutor p):BaseRepository(p),IProcurementOfficerWorkflowRepository
{
 public Task<IReadOnlyList<ProcurementOfficerRequisitionDto>>GetAssignedAsync(string e,CancellationToken c)=>StoredProcedures.QueryAsync("dbo.usp_ProcurementOfficer_GetAssigned",r=>new ProcurementOfficerRequisitionDto(r.GetInt64(r.GetOrdinal("REQUISITION_ID")),r.GetString(r.GetOrdinal("REQUISITION_NO")),r.GetDateTime(r.GetOrdinal("REQUESTED_DATE")),r.GetDecimal(r.GetOrdinal("TOTAL_ESTIMATED_AMOUNT")),r.GetString(r.GetOrdinal("CURRENT_STATUS")),r.GetInt32(r.GetOrdinal("ITEM_COUNT")),r.GetNullableString("ASSIGNED_TO_EMPLOYEE_ID"),r.GetBoolean(r.GetOrdinal("IS_OWNED_BY_CURRENT_USER")),r.GetString(r.GetOrdinal("ASSET_NAMES")),r.GetString(r.GetOrdinal("ORG_UNIT_NAME")),r.GetString(r.GetOrdinal("REQUESTED_BY_EMPLOYEE_ID")),r.GetString(r.GetOrdinal("REQUESTED_BY_NAME")),r.GetString(r.GetOrdinal("REQUISITION_TYPES"))),[I("@EmployeeId",SqlDbType.VarChar,e,30)],c);
 public async Task<RequisitionDetailDto?>GetDetailAsync(long id,string e,CancellationToken c)=>RequisitionDetailDataSetMapper.Map(await StoredProcedures.QueryDataSetAsync("dbo.usp_ProcurementOfficer_GetRequisitionDetail",[I("@RequisitionId",SqlDbType.BigInt,id),I("@EmployeeId",SqlDbType.VarChar,e,30)],c));
 public Task<StoredProcedureResult?>TakeAsync(long id,string e,string?m,CancellationToken c)=>Run("dbo.usp_ProcurementOfficer_TakeForReview",id,e,m,c);
 public Task<StoredProcedureResult?>ApproveAsync(long id,string e,string?m,CancellationToken c)=>Run("dbo.usp_ProcurementOfficer_Approve",id,e,m,c);
 public Task<StoredProcedureResult?>ReturnAsync(long id,string e,string m,CancellationToken c)=>Run("dbo.usp_ProcurementOfficer_Return",id,e,m,c);
 public Task<StoredProcedureResult?>RejectAsync(long id,string e,string m,CancellationToken c)=>Run("dbo.usp_ProcurementOfficer_Reject",id,e,m,c);
 public Task<StoredProcedureResult?>AddCommentAsync(long id,long?item,string text,string e,CancellationToken c)=>StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_ProcurementOfficer_AddComment",StoredProcedureResult.FromReader,[I("@RequisitionId",SqlDbType.BigInt,id),I("@ReqItemId",SqlDbType.BigInt,item),I("@CommentText",SqlDbType.NVarChar,text,4000),I("@ActorEmployeeId",SqlDbType.VarChar,e,30)],c);
 private Task<StoredProcedureResult?>Run(string n,long id,string e,string?m,CancellationToken c)=>StoredProcedures.QuerySingleOrDefaultAsync(n,StoredProcedureResult.FromReader,[I("@RequisitionId",SqlDbType.BigInt,id),I("@ActorEmployeeId",SqlDbType.VarChar,e,30),I("@Remarks",SqlDbType.NVarChar,m,2000),I("@BatchReferenceId",SqlDbType.UniqueIdentifier,null)],c);
 private static SqlParameter I(string n,SqlDbType t,object?v,int?s=null)=>SqlParameterFactory.Input(n,t,v,s);
}
