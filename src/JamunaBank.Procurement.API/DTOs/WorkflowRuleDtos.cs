using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record WorkflowRuleDto(long Id,string Code,string Name,long AssetId,string? AssetCode,string? AssetName,string RequisitionType,decimal? MinimumAmount,decimal? MaximumAmount,long? ConcernDivisionId,string? ConcernDivisionCode,string? ConcernDivisionName,bool AuthorityBeforeDivision,string DestinationAfterConcern,int PriorityOrder,bool IsActive,DateOnly EffectiveFrom,DateOnly? EffectiveTo);
public sealed record SaveWorkflowRuleRequest
{
 [Required,StringLength(40)] public string Code{get;init;}=string.Empty;
 [Required,StringLength(200)] public string Name{get;init;}=string.Empty;
 [Range(1,long.MaxValue)] public long AssetId{get;init;}
 [Required] public string RequisitionType{get;init;}=string.Empty;
 public decimal? MinimumAmount{get;init;} public decimal? MaximumAmount{get;init;}
 public long? ConcernDivisionId{get;init;} public bool AuthorityBeforeDivision{get;init;}
 [Required] public string DestinationAfterConcern{get;init;}=string.Empty;
 public int PriorityOrder{get;init;}=100; public bool IsActive{get;init;}=true;
 public DateOnly EffectiveFrom{get;init;}=DateOnly.FromDateTime(DateTime.UtcNow); public DateOnly? EffectiveTo{get;init;}
}
public sealed class EvaluateWorkflowRuleRequest
{
 [Range(1,long.MaxValue)] public long AssetId{get;init;}
 [Required] public string RequisitionType{get;init;}=string.Empty;
 [Range(typeof(decimal),"0","79228162514264337593543950335")] public decimal Amount{get;init;}
}
public sealed record WorkflowEvaluationDto(WorkflowRuleDto MatchedRule,ConcernDivisionReferenceDto? ConcernDivision,bool AuthorityBeforeDivision,string DestinationAfterConcern,string CalculatedRoute);
public sealed record ConcernDivisionReferenceDto(long Id,string? Code,string? Name);
