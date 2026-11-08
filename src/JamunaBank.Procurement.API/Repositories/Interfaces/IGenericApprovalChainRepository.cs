using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Repositories.Interfaces;

public interface IGenericApprovalChainRepository
{
    Task<IReadOnlyList<ApprovalUnitDto>> GetUnitsAsync(bool activeOnly,CancellationToken ct);
    Task<StoredProcedureResult?> SaveUnitAsync(long? id,SaveApprovalUnitRequest request,string actor,CancellationToken ct);
    Task<IReadOnlyList<ApprovalUnitApproverDto>> GetApproversAsync(long unitId,bool activeOnly,CancellationToken ct);
    Task<StoredProcedureResult?> SaveApproverAsync(SaveApprovalUnitApproverRequest request,string actor,CancellationToken ct);
    Task<IReadOnlyList<WorkflowRuleStepDto>> GetRuleStepsAsync(long ruleId,CancellationToken ct);
    Task<StoredProcedureResult?> SaveRuleStepAsync(long? id,SaveWorkflowRuleStepRequest request,string actor,CancellationToken ct);
    Task<StoredProcedureResult?> InitializeAsync(long workflowInstanceId,string actor,CancellationToken ct);
    Task<IReadOnlyList<PendingGenericApprovalDto>> GetPendingAsync(string employeeId,CancellationToken ct);
    Task<IReadOnlyList<ApprovalChainEntryDto>> GetByRequisitionAsync(long requisitionId,CancellationToken ct);
    Task<StoredProcedureResult?> ActAsync(long stepId,string employeeId,GenericApprovalActionRequest request,CancellationToken ct);
}
