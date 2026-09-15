using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IProcurementOfficerWorkflowRepository
{
 Task<IReadOnlyList<ProcurementOfficerRequisitionDto>> GetAssignedAsync(string employeeId,CancellationToken ct);
 Task<RequisitionDetailDto?> GetDetailAsync(long id,string employeeId,CancellationToken ct);
 Task<StoredProcedureResult?> TakeAsync(long id,string employeeId,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> ApproveAsync(long id,string employeeId,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> ReturnAsync(long id,string employeeId,string remarks,CancellationToken ct);
 Task<StoredProcedureResult?> RejectAsync(long id,string employeeId,string remarks,CancellationToken ct);
 Task<StoredProcedureResult?> AddCommentAsync(long id,long? itemId,string text,string employeeId,CancellationToken ct);
}
