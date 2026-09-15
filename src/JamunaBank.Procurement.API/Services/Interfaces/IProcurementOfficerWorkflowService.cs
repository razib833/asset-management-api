using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IProcurementOfficerWorkflowService
{
 Task<IReadOnlyList<ProcurementOfficerRequisitionDto>> GetAssignedAsync(CancellationToken ct);
 Task<RequisitionDetailDto?> GetDetailAsync(long id,CancellationToken ct);
 Task<StoredProcedureResult?> TakeAsync(long id,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> ApproveAsync(long id,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> ReturnAsync(long id,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> RejectAsync(long id,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> AddCommentAsync(long id,ProcurementOfficerCommentRequest request,CancellationToken ct);
}
