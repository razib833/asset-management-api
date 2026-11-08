using System.ComponentModel.DataAnnotations;

namespace JamunaBank.Procurement.API.DTOs;

public sealed record ApprovalUnitDto(long Id,long? ParentId,long? OrgUnitId,string Code,string Name,string Type,bool IsActive,string? ParentName,string? OrgUnitCode,string? OrgUnitName,int ActiveApproverCount);
public sealed record ApprovalUnitApproverDto(long Id,long ApprovalUnitId,string EmployeeId,string DisplayName,string? Email,string? Designation,string RoleCode,int AuthorityLevel,DateOnly? EffectiveFrom,DateOnly? EffectiveTo,bool IsActive);
public sealed record WorkflowRuleStepDto(long Id,long WorkflowRuleId,int StepOrder,string StepCode,string StepName,string ApproverSource,long? ApprovalUnitId,string? ApprovalUnitCode,string? ApprovalUnitName,string? EmployeeId,string? EmployeeName,string? ApproverRoleCode,string ApprovalMode,int RequiredApprovalCount,bool IsOptional,string? CompletionDestination,bool IsActive);
public sealed record PendingGenericApprovalDto(long RequisitionId,string RequisitionNo,long WorkflowInstanceStepId,int StepOrder,string StepCode,string StepName,string ApprovalMode,int RequiredApprovalCount,DateTime? ActivatedOn);
public sealed record ApprovalChainEntryDto(long WorkflowInstanceStepId,long WorkflowInstanceId,int StepOrder,string StepCode,string StepName,string ApproverSource,string ApprovalMode,int RequiredApprovalCount,bool IsOptional,string? CompletionDestination,string StepStatus,DateTime? ActivatedOn,DateTime? CompletedOn,string? EmployeeId,string? EmployeeName,int? AuthorityLevel,string? DecisionStatus,DateTime? DecisionOn,string? Remarks);

public sealed class SaveApprovalUnitRequest
{
    public long? ParentApprovalUnitId { get; init; }
    public long? OrgUnitId { get; init; }
    [Required,StringLength(50)] public string Code { get; init; } = string.Empty;
    [Required,StringLength(150)] public string Name { get; init; } = string.Empty;
    [Required,StringLength(20)] public string Type { get; init; } = string.Empty;
    public bool IsActive { get; init; } = true;
}

public sealed class SaveApprovalUnitApproverRequest
{
    public long? Id { get; init; }
    [Range(1,long.MaxValue)] public long ApprovalUnitId { get; init; }
    [Required,StringLength(30)] public string EmployeeId { get; init; } = string.Empty;
    [Required,StringLength(50)] public string RoleCode { get; init; } = string.Empty;
    [Range(1,int.MaxValue)] public int AuthorityLevel { get; init; } = 1;
    public DateOnly? EffectiveFrom { get; init; }
    public DateOnly? EffectiveTo { get; init; }
    public bool IsActive { get; init; } = true;
}

public sealed class SaveWorkflowRuleStepRequest
{
    [Range(1,long.MaxValue)] public long WorkflowRuleId { get; init; }
    [Range(1,int.MaxValue)] public int StepOrder { get; init; }
    [Required,StringLength(50)] public string StepCode { get; init; } = string.Empty;
    [Required,StringLength(150)] public string StepName { get; init; } = string.Empty;
    [Required,StringLength(40)] public string ApproverSource { get; init; } = string.Empty;
    public long? ApprovalUnitId { get; init; }
    [StringLength(30)] public string? EmployeeId { get; init; }
    [StringLength(50)] public string? ApproverRoleCode { get; init; }
    [Required,StringLength(15)] public string ApprovalMode { get; init; } = "ANY_ONE";
    [Range(1,int.MaxValue)] public int RequiredApprovalCount { get; init; } = 1;
    public bool IsOptional { get; init; }
    [StringLength(50)] public string? CompletionDestination { get; init; }
    public bool IsActive { get; init; } = true;
}

public sealed class GenericApprovalActionRequest
{
    [Required,StringLength(15)] public string ActionType { get; init; } = string.Empty;
    [StringLength(1000)] public string? Remarks { get; init; }
    public Guid? BatchReference { get; init; }
}
