using System.Data;using JamunaBank.Procurement.API.Data;using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Repositories.Base;using JamunaBank.Procurement.API.Repositories.Interfaces;using Microsoft.Data.SqlClient;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class ProcurementTrackingRepository(IStoredProcedureExecutor p):BaseRepository(p),IProcurementTrackingRepository
{
 public Task<IReadOnlyList<ProcurementStatusDto>>GetStatusesAsync(CancellationToken c)=>StoredProcedures.QueryAsync<ProcurementStatusDto>("dbo.usp_ProcurementTracking_GetStatuses",r=>new(r.GetString(0),r.GetString(1),r.GetInt32(2)),null,c);
 public Task<IReadOnlyList<ProcurementTrackingSearchDto>>SearchAsync(string?n,long?o,long?a,string?s,CancellationToken c)=>StoredProcedures.QueryAsync<ProcurementTrackingSearchDto>("dbo.usp_ProcurementTracking_Search",r=>
 {
  // Read in result-set ordinal order because QueryAsync uses SequentialAccess.
  var procurementId=r.GetInt64(0);var requisitionId=r.GetInt64(1);var requisitionNo=r.GetString(2);
  var requestedOrgUnitId=r.GetInt64(3);var orgUnitName=r.GetString(4);var totalEstimatedAmount=r.GetDecimal(5);
  var assets=r.IsDBNull(6)?string.Empty:r.GetString(6);var requisitionTypes=r.IsDBNull(7)?string.Empty:r.GetString(7);
  var statusCode=r.GetString(8);var statusName=r.GetString(9);var referenceNo=r.IsDBNull(10)?null:r.GetString(10);
  var modifiedDate=r.GetDateTime(11);
  return new(procurementId,requisitionId,requisitionNo,requestedOrgUnitId,orgUnitName,assets,requisitionTypes,totalEstimatedAmount,statusCode,statusName,referenceNo,modifiedDate);
 },[I("@RequisitionNo",SqlDbType.VarChar,n,30),I("@OrgUnitId",SqlDbType.BigInt,o),I("@AssetId",SqlDbType.BigInt,a),I("@StatusCode",SqlDbType.VarChar,s,40)],c);
 public async Task<ProcurementTrackingDetailDto?>GetAsync(long id,CancellationToken c){var ds=await StoredProcedures.QueryDataSetAsync("dbo.usp_ProcurementTracking_GetDetail",[I("@RequisitionId",SqlDbType.BigInt,id)],c);if(ds.Tables.Count==0||ds.Tables[0].Rows.Count==0)return null;var x=ds.Tables[0].Rows[0];var history=ds.Tables.Count<2?[]:ds.Tables[1].Rows.Cast<DataRow>().Select(h=>new ProcurementStatusHistoryDto((long)h["STATUS_HISTORY_ID"],(string)h["STATUS_CODE"],(string)h["STATUS_NAME"],(DateTime)h["STATUS_DATE"],h["REFERENCE_NO"]as string,(string)h["CHANGED_BY_EMPLOYEE_ID"],h["REMARKS"]as string,h["BATCH_REFERENCE_ID"]==DBNull.Value?null:(Guid)h["BATCH_REFERENCE_ID"])).ToArray();return new((long)x["PROCUREMENT_ID"],(long)x["REQUISITION_ID"],(string)x["REQUISITION_NO"],(string)x["ORG_UNIT_NAME"],(string)x["STATUS_CODE"],(string)x["STATUS_NAME"],x["PROCUREMENT_REFERENCE_NO"]as string,x["REMARKS"]as string,history);}
 public Task<StoredProcedureResult?>UpdateStatusAsync(long id,string s,DateTime d,string?r,string?m,string e,Guid?b,CancellationToken c)=>StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_ProcurementTracking_UpdateStatus",StoredProcedureResult.FromReader,[I("@RequisitionId",SqlDbType.BigInt,id),I("@StatusCode",SqlDbType.VarChar,s,40),I("@StatusDate",SqlDbType.DateTime2,d),I("@ActorEmployeeId",SqlDbType.VarChar,e,30),I("@ReferenceNo",SqlDbType.VarChar,r,100),I("@Remarks",SqlDbType.NVarChar,m,2000),I("@BatchReferenceId",SqlDbType.UniqueIdentifier,b)],c);private static SqlParameter I(string n,SqlDbType t,object?v,int?s=null)=>SqlParameterFactory.Input(n,t,v,s);
}
