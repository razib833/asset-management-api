using System.Data;using JamunaBank.Procurement.API.Data;using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Repositories.Base;using JamunaBank.Procurement.API.Repositories.Interfaces;using Microsoft.Data.SqlClient;
namespace JamunaBank.Procurement.API.Repositories.Implementations;
public sealed class ProcurementDashboardRepository(IStoredProcedureExecutor procedures):BaseRepository(procedures),IProcurementDashboardRepository
{
 public async Task<ProcurementDashboardDto>GetAsync(int year,CancellationToken ct)
 {
  var ds=await StoredProcedures.QueryDataSetAsync("dbo.usp_ProcurementDashboard_Get",[SqlParameterFactory.Input("@Year",SqlDbType.Int,year)],ct);
  if(ds.Tables.Count<5||ds.Tables[0].Rows.Count==0)throw new InvalidOperationException("The dashboard procedure returned an incomplete result.");
  var s=ds.Tables[0].Rows[0];
  var summary=new ProcurementDashboardSummaryDto(Convert.ToInt32(s["PENDING_REQUISITIONS"]),Convert.ToInt32(s["READY_FOR_PROCUREMENT"]),Convert.ToInt32(s["UNDER_PROCUREMENT"]),Convert.ToInt32(s["PROCUREMENT_COMPLETED"]),Convert.ToInt32(s["SELECTED_YEAR"]));
  var stages=ds.Tables[1].Rows.Cast<DataRow>().Select(x=>new PendingStageDto(x["STAGE_CODE"]==DBNull.Value?"UNASSIGNED":(string)x["STAGE_CODE"],(string)x["STAGE_NAME"],Convert.ToInt32(x["STAGE_COUNT"]))).ToArray();
  var trend=ds.Tables[2].Rows.Cast<DataRow>().Select(x=>new MonthlyProcurementTrendDto(Convert.ToInt32(x["MONTH_NO"]),(string)x["MONTH_NAME"],Convert.ToInt32(x["RECEIVED_COUNT"]),Convert.ToInt32(x["COMPLETED_COUNT"]))).ToArray();
  var ready=ds.Tables[3].Rows.Cast<DataRow>().Select(x=>new RecentlyReadyDto((long)x["REQUISITION_ID"],(string)x["REQUISITION_NO"],(string)x["ASSET_NAME"],(string)x["REQUESTING_UNIT"],(string)x["REQUISITION_TYPE"],(DateTime)x["READY_DATE"],Convert.ToInt32(x["READY_SINCE_DAYS"]))).ToArray();
  var categories=ds.Tables[4].Rows.Cast<DataRow>().Select(x=>new AssetCategoryBreakdownDto((long)x["ASSET_CATEGORY_ID"],(string)x["CATEGORY_NAME"],Convert.ToInt32(x["CATEGORY_COUNT"]),Convert.ToDecimal(x["PERCENTAGE"]))).ToArray();
  return new(summary,stages,trend,ready,categories);
 }
}
