using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IWorkflowRuleRepository
{
 Task<IReadOnlyList<WorkflowRuleDto>> GetAllAsync(bool activeOnly,CancellationToken ct);Task<WorkflowRuleDto?> GetByIdAsync(long id,CancellationToken ct);Task<IReadOnlyList<WorkflowRuleDto>> GetByAssetAsync(long assetId,bool activeOnly,CancellationToken ct);Task<StoredProcedureResult?> SaveAsync(long? id,SaveWorkflowRuleRequest request,string actor,CancellationToken ct);Task<WorkflowEvaluationDto?> EvaluateAsync(EvaluateWorkflowRuleRequest request,CancellationToken ct);
}
