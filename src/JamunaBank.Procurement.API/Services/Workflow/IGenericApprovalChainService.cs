using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Services.Interfaces;

public interface IGenericApprovalChainService
{
    Task<IReadOnlyList<ApprovalUnitDto>> GetUnitsAsync(bool activeOnly,CancellationToken ct);
    Task<StoredProcedureResult?> SaveUnitAsync(long? id,SaveApprovalUnitRequest request,CancellationToken ct);
    Task<IReadOnlyList<ApprovalUnitApproverDto>> GetApproversAsync(long unitId,bool activeOnly,CancellationToken ct);
    Task<StoredProcedureResult?> SaveApproverAsync(SaveApprovalUnitApproverRequest request,CancellationToken ct);
    Task<IReadOnlyList<WorkflowRuleStepDto>> GetRuleStepsAsync(long ruleId,CancellationToken ct);
    Task<StoredProcedureResult?> SaveRuleStepAsync(long? id,SaveWorkflowRuleStepRequest request,CancellationToken ct);
    Task<StoredProcedureResult?> InitializeAsync(long workflowInstanceId,CancellationToken ct);
    Task<IReadOnlyList<PendingGenericApprovalDto>> GetMyPendingAsync(CancellationToken ct);
    Task<RequisitionDetailDto?> GetReviewDetailAsync(long workflowInstanceStepId,CancellationToken ct);
    Task<IReadOnlyList<ApprovalChainEntryDto>> GetByRequisitionAsync(long requisitionId,CancellationToken ct);
    Task<StoredProcedureResult?> ActAsync(long stepId,GenericApprovalActionRequest request,CancellationToken ct);
}
