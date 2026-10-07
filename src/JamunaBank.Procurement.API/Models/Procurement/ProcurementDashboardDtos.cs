namespace JamunaBank.Procurement.API.DTOs;
public sealed record ProcurementDashboardSummaryDto(int PendingRequisitions,int ReadyForProcurement,int UnderProcurement,int ProcurementCompleted,int SelectedYear);
public sealed record PendingStageDto(string StageCode,string StageName,int Count);
public sealed record MonthlyProcurementTrendDto(int Month,string MonthName,int Received,int Completed);
public sealed record RecentlyReadyDto(long RequisitionId,string RequisitionNo,string AssetName,string RequestingUnit,string RequisitionType,DateTime ReadyForProcurementDate,int ReadySinceDays);
public sealed record AssetCategoryBreakdownDto(long CategoryId,string CategoryName,int Count,decimal Percentage);
public sealed record ProcurementDashboardDto(ProcurementDashboardSummaryDto Summary,IReadOnlyList<PendingStageDto> PendingByStage,IReadOnlyList<MonthlyProcurementTrendDto> MonthlyTrend,IReadOnlyList<RecentlyReadyDto> RecentlyReady,IReadOnlyList<AssetCategoryBreakdownDto> AssetCategoryBreakdown);
