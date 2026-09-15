using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IConcernAuthorityRepository
{
 Task<IReadOnlyList<ConcernAuthorityPendingDto>> GetPendingAsync(string employeeId,CancellationToken ct);
 Task<RequisitionDetailDto?> GetDetailAsync(long id,string employeeId,CancellationToken ct);
 Task<ConcernAuthorityReviewContextDto?> GetContextAsync(long id,string employeeId,CancellationToken ct);
 Task<StoredProcedureResult?> ApproveAsync(long id,string employeeId,string? remarks,Guid? batchId,CancellationToken ct);
 Task<StoredProcedureResult?> ReturnAsync(long id,string employeeId,string remarks,CancellationToken ct);
 Task<StoredProcedureResult?> RejectAsync(long id,string employeeId,string remarks,Guid? batchId,CancellationToken ct);
}
