using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ConcernDivisionPendingDto(long Id,string Number,DateTime RequestedDate,decimal TotalEstimatedAmount,long ConcernDivisionId,string ConcernDivisionName,string RequestedByName,int ItemCount);
public sealed record ConcernDivisionReviewContextDto(long WorkflowRuleId,string RuleCode,string RuleName,string CurrentStage,string MatrixDestination,long ConcernDivisionId,string ConcernDivisionName);
public sealed class ConcernDivisionActionRequest{[StringLength(2000)]public string? Remarks{get;init;}}
