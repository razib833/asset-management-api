using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ConcernAuthorityPendingDto(long Id,string Number,DateTime RequestedDate,decimal TotalEstimatedAmount,long ConcernDivisionId,string ConcernDivisionName,string RequestedByName,int ItemCount);
public sealed record ConcernAuthorityReviewContextDto(long WorkflowRuleId,string RuleCode,string RuleName,string CurrentStage,string NextDestination,long ConcernDivisionId,string ConcernDivisionName,string ConcernOfficialEmployeeId,string? ConcernOfficialName,string? ConcernOfficialRemarks,DateTime ForwardedDate);
public sealed class ConcernAuthorityActionRequest{[StringLength(2000)]public string? Remarks{get;init;}}
public sealed class ConcernAuthorityBulkApproveRequest{[Required,MinLength(1)]public IReadOnlyCollection<long> RequisitionIds{get;init;}=[];[StringLength(2000)]public string? Remarks{get;init;}}
public sealed record ConcernAuthorityBulkItemResult(long RequisitionId,string? RequisitionNo,string ResultCode,string ResultMessage,string? NextDestination);
public sealed record ConcernAuthorityBulkResult(Guid BatchReferenceId,int TotalSelected,int SuccessfulCount,int FailedCount,IReadOnlyList<ConcernAuthorityBulkItemResult> Results);
