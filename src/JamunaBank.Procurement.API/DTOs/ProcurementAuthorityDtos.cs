using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ProcurementAuthorityPendingDto(long Id,string Number,DateTime RequestedDate,decimal TotalEstimatedAmount,string RequestedByName,int ItemCount);
public sealed class ProcurementAuthorityActionRequest{[StringLength(2000)]public string? Remarks{get;init;}}
public sealed class ProcurementAuthorityBulkApproveRequest{[Required,MinLength(1)]public IReadOnlyCollection<long> RequisitionIds{get;init;}=[];[StringLength(2000)]public string? Remarks{get;init;}}
public sealed record ProcurementAuthorityBulkItemResult(long RequisitionId,string? RequisitionNo,string ResultCode,string ResultMessage,string? NextDestination);
public sealed record ProcurementAuthorityBulkResult(Guid BatchReferenceId,int TotalSelected,int SuccessfulCount,int FailedCount,IReadOnlyList<ProcurementAuthorityBulkItemResult> Results);
public sealed record ProcurementAuthorityReviewContextDto(long WorkflowRuleId,string RuleCode,string RuleName,string CurrentStage,string NextStepAfterApproval,bool CanForward,string? ConcernDivisionName);
