using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class ProcurementOfficerWorkflowService(IProcurementOfficerWorkflowRepository repository,ICurrentUserService currentUser):IProcurementOfficerWorkflowService
{
 public Task<IReadOnlyList<ProcurementOfficerRequisitionDto>>GetAssignedAsync(CancellationToken c)=>repository.GetAssignedAsync(currentUser.EmployeeId,c);
 public Task<RequisitionDetailDto?>GetDetailAsync(long id,CancellationToken c)=>repository.GetDetailAsync(Valid(id),currentUser.EmployeeId,c);
 public Task<StoredProcedureResult?>TakeAsync(long id,string?m,CancellationToken c){Length(m);return repository.TakeAsync(Valid(id),currentUser.EmployeeId,m?.Trim(),c);}
 public Task<StoredProcedureResult?>ApproveAsync(long id,string?m,CancellationToken c){Length(m);return repository.ApproveAsync(Valid(id),currentUser.EmployeeId,m?.Trim(),c);}
 public Task<StoredProcedureResult?>ReturnAsync(long id,string?m,CancellationToken c)=>repository.ReturnAsync(Valid(id),currentUser.EmployeeId,Required(m),c);
 public Task<StoredProcedureResult?>RejectAsync(long id,string?m,CancellationToken c)=>repository.RejectAsync(Valid(id),currentUser.EmployeeId,Required(m),c);
 public Task<StoredProcedureResult?>AddCommentAsync(long id,ProcurementOfficerCommentRequest r,CancellationToken c){Valid(id);if(r.RequisitionItemId<=0)throw new ArgumentOutOfRangeException(nameof(r.RequisitionItemId));var text=r.CommentText?.Trim();if(string.IsNullOrWhiteSpace(text))throw new ArgumentException("Comment text is required.");return repository.AddCommentAsync(id,r.RequisitionItemId,text,currentUser.EmployeeId,c);}
 private static long Valid(long id){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));return id;}
 private static string Required(string?m){if(string.IsNullOrWhiteSpace(m))throw new ArgumentException("Remarks are required for return and reject.");Length(m);return m.Trim();}
 private static void Length(string?m){if(m?.Length>2000)throw new ArgumentException("Remarks cannot exceed 2000 characters.");}
}
