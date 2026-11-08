using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ProcurementTrackingSearchDto(long ProcurementId,long RequisitionId,string RequisitionNo,long RequestedOrgUnitId,string OrgUnitName,string Assets,string RequisitionTypes,decimal TotalEstimatedAmount,string StatusCode,string StatusName,string? ReferenceNo,DateTime ModifiedDate);
public sealed record ProcurementStatusDto(string Code,string Name,int DisplayOrder);
public sealed record ProcurementStatusHistoryDto(long Id,string StatusCode,string StatusName,DateTime StatusDate,string? ReferenceNo,string ChangedByEmployeeId,string? Remarks,Guid? BatchReferenceId);
public sealed record ProcurementTrackingDetailDto(long ProcurementId,long RequisitionId,string RequisitionNo,string OrgUnitName,string StatusCode,string StatusName,string? ReferenceNo,string? Remarks,IReadOnlyList<ProcurementStatusHistoryDto> History);
public sealed class ProcurementTrackingUpdateRequest{[Required,MinLength(1)]public IReadOnlyCollection<long> RequisitionIds{get;init;}=[];[Required,StringLength(40)]public string NewStatusCode{get;init;}=string.Empty;public DateTime StatusDate{get;init;}[StringLength(100)]public string? ReferenceNo{get;init;}[StringLength(2000)]public string? Remarks{get;init;}}
public sealed record ProcurementTrackingUpdateItemResult(long RequisitionId,string? RequisitionNo,string ResultCode,string ResultMessage,string? NewStatusCode);
public sealed record ProcurementTrackingBulkResult(Guid BatchReferenceId,int TotalSelected,int SuccessfulCount,int FailedCount,IReadOnlyList<ProcurementTrackingUpdateItemResult> Results);
