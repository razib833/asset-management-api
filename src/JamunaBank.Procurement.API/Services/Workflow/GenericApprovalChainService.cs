using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;

namespace JamunaBank.Procurement.API.Services.Implementations;

public sealed class GenericApprovalChainService(IGenericApprovalChainRepository repository,ICurrentUserService currentUser) : IGenericApprovalChainService
{
    private static readonly HashSet<string> UnitTypes=new(["DIVISION","DEPARTMENT","UNIT","SECTION","TEAM"]);
    private static readonly HashSet<string> Sources=new(["EMPLOYEE","APPROVAL_UNIT","ROLE_POOL","REQUESTING_UNIT_MANAGER","CONCERN_DIVISION_OFFICIAL","PROCUREMENT_AUTHORITY"]);
    private static readonly HashSet<string> Modes=new(["ANY_ONE","ALL","COUNT"]);
    private static readonly HashSet<string> Actions=new(["APPROVE","RETURN","REJECT","SKIP"]);
    public Task<IReadOnlyList<ApprovalUnitDto>> GetUnitsAsync(bool activeOnly,CancellationToken ct)=>repository.GetUnitsAsync(activeOnly,ct);
    public Task<StoredProcedureResult?> SaveUnitAsync(long? id,SaveApprovalUnitRequest x,CancellationToken ct){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));if(!UnitTypes.Contains(x.Type.Trim().ToUpperInvariant()))throw new ArgumentException("Unit type must be DIVISION, DEPARTMENT, UNIT, SECTION, or TEAM.");if(x.ParentApprovalUnitId==id)throw new ArgumentException("An approval unit cannot be its own parent.");return repository.SaveUnitAsync(id,x,currentUser.EmployeeId,ct);}
    public Task<IReadOnlyList<ApprovalUnitApproverDto>> GetApproversAsync(long id,bool activeOnly,CancellationToken ct){Positive(id);return repository.GetApproversAsync(id,activeOnly,ct);}
    public Task<StoredProcedureResult?> SaveApproverAsync(SaveApprovalUnitApproverRequest x,CancellationToken ct){Positive(x.ApprovalUnitId);if(x.EffectiveTo<x.EffectiveFrom)throw new ArgumentException("Effective-to date cannot precede effective-from date.");return repository.SaveApproverAsync(x,currentUser.EmployeeId,ct);}
    public Task<IReadOnlyList<WorkflowRuleStepDto>> GetRuleStepsAsync(long id,CancellationToken ct){Positive(id);return repository.GetRuleStepsAsync(id,ct);}
    public Task<StoredProcedureResult?> SaveRuleStepAsync(long? id,SaveWorkflowRuleStepRequest x,CancellationToken ct){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));Positive(x.WorkflowRuleId);var source=x.ApproverSource.Trim().ToUpperInvariant();if(!Sources.Contains(source))throw new ArgumentException("Invalid approver source.");if(!Modes.Contains(x.ApprovalMode.Trim().ToUpperInvariant()))throw new ArgumentException("Approval mode must be ANY_ONE, ALL, or COUNT.");if(source=="EMPLOYEE"&&string.IsNullOrWhiteSpace(x.EmployeeId))throw new ArgumentException("Employee ID is required for an employee step.");if(source=="APPROVAL_UNIT"&&x.ApprovalUnitId is null or <=0)throw new ArgumentException("Approval unit is required for an approval-unit step.");if(source=="ROLE_POOL"&&string.IsNullOrWhiteSpace(x.ApproverRoleCode))throw new ArgumentException("Role code is required for a role-pool step.");return repository.SaveRuleStepAsync(id,x,currentUser.EmployeeId,ct);}
    public Task<StoredProcedureResult?> InitializeAsync(long id,CancellationToken ct){Positive(id);return repository.InitializeAsync(id,currentUser.EmployeeId,ct);}
    public Task<IReadOnlyList<PendingGenericApprovalDto>> GetMyPendingAsync(CancellationToken ct)=>repository.GetPendingAsync(currentUser.EmployeeId,ct);
    public Task<RequisitionDetailDto?> GetReviewDetailAsync(long id,CancellationToken ct){Positive(id);return repository.GetReviewDetailAsync(id,currentUser.EmployeeId,ct);}
    public Task<IReadOnlyList<ApprovalChainEntryDto>> GetByRequisitionAsync(long id,CancellationToken ct){Positive(id);return repository.GetByRequisitionAsync(id,ct);}
    public Task<StoredProcedureResult?> ActAsync(long id,GenericApprovalActionRequest x,CancellationToken ct){Positive(id);if(!Actions.Contains(x.ActionType.Trim().ToUpperInvariant()))throw new ArgumentException("Action must be APPROVE, RETURN, REJECT, or SKIP.");return repository.ActAsync(id,currentUser.EmployeeId,x,ct);}
    private static void Positive(long value){if(value<=0)throw new ArgumentOutOfRangeException(nameof(value));}
}
