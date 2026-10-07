using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IConcernAuthorityService
{
 Task<IReadOnlyList<ConcernAuthorityPendingDto>> GetPendingAsync(CancellationToken ct);
 Task<IReadOnlyList<BranchRequisitionDto>> GetAllAsync(string? requisitionNo,long? assetId,string? requisitionType,string? status,DateOnly? fromDate,DateOnly? toDate,CancellationToken ct);
 Task<RequisitionDetailDto?> GetAllDetailAsync(long id,CancellationToken ct);
 Task<RequisitionDetailDto?> GetDetailAsync(long id,CancellationToken ct);
 Task<ConcernAuthorityReviewContextDto?> GetContextAsync(long id,CancellationToken ct);
 Task<StoredProcedureResult?> ApproveAsync(long id,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> ReturnAsync(long id,string? remarks,CancellationToken ct);
 Task<StoredProcedureResult?> RejectAsync(long id,string? remarks,CancellationToken ct);
 Task<ConcernAuthorityBulkResult> BulkApproveAsync(ConcernAuthorityBulkApproveRequest request,CancellationToken ct);
 Task<ConcernAuthorityBulkResult> BulkRejectAsync(ConcernAuthorityBulkApproveRequest request,CancellationToken ct);
}
