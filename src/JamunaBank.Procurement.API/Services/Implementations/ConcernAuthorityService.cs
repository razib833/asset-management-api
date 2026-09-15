using JamunaBank.Procurement.API.Constants;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class ConcernAuthorityService(IConcernAuthorityRepository repository,ICurrentUserService currentUser):IConcernAuthorityService
{
 public Task<IReadOnlyList<ConcernAuthorityPendingDto>> GetPendingAsync(CancellationToken c)=>repository.GetPendingAsync(currentUser.EmployeeId,c);
 public Task<RequisitionDetailDto?> GetDetailAsync(long id,CancellationToken c)=>repository.GetDetailAsync(Valid(id),currentUser.EmployeeId,c);
 public Task<ConcernAuthorityReviewContextDto?> GetContextAsync(long id,CancellationToken c)=>repository.GetContextAsync(Valid(id),currentUser.EmployeeId,c);
 public Task<StoredProcedureResult?> ApproveAsync(long id,string?m,CancellationToken c){Length(m);return repository.ApproveAsync(Valid(id),currentUser.EmployeeId,m?.Trim(),null,c);}
 public Task<StoredProcedureResult?> ReturnAsync(long id,string?m,CancellationToken c)=>repository.ReturnAsync(Valid(id),currentUser.EmployeeId,Required(m),c);
 public Task<StoredProcedureResult?> RejectAsync(long id,string?m,CancellationToken c)=>repository.RejectAsync(Valid(id),currentUser.EmployeeId,Required(m),null,c);
 public Task<ConcernAuthorityBulkResult> BulkApproveAsync(ConcernAuthorityBulkApproveRequest request,CancellationToken c)=>Bulk(request,false,c);
 public Task<ConcernAuthorityBulkResult> BulkRejectAsync(ConcernAuthorityBulkApproveRequest request,CancellationToken c)=>Bulk(request,true,c);
 private async Task<ConcernAuthorityBulkResult> Bulk(ConcernAuthorityBulkApproveRequest request,bool reject,CancellationToken c)
 {
  if(request.RequisitionIds is null||request.RequisitionIds.Count==0)throw new ArgumentException("At least one requisition ID is required.");
  var remarks=reject?Required(request.Remarks):request.Remarks?.Trim();Length(remarks);
  var ids=request.RequisitionIds.Distinct().ToArray();if(ids.Any(x=>x<=0))throw new ArgumentException("Requisition IDs must be positive.");
  var pending=(await repository.GetPendingAsync(currentUser.EmployeeId,c)).ToDictionary(x=>x.Id,x=>x.Number);var batch=Guid.NewGuid();var rows=new List<ConcernAuthorityBulkItemResult>(ids.Length);
  foreach(var id in ids)
  {
   StoredProcedureResult? result;
   try{result=reject?await repository.RejectAsync(id,currentUser.EmployeeId,remarks!,batch,c):await repository.ApproveAsync(id,currentUser.EmployeeId,remarks,batch,c);}
   catch(OperationCanceledException){throw;}
   catch(Exception){result=new(ResultCodes.UnexpectedError,reject?"The requisition could not be rejected.":"The requisition could not be processed.",id,null);}
   result??=new(ResultCodes.UnexpectedError,"No stored-procedure result was returned.",id,null);pending.TryGetValue(id,out var number);
   rows.Add(new(id,number,result.ResultCode,result.ResultMessage,reject&&result.ResultCode==ResultCodes.Success?"REJECTED":result.ReferenceNo));
  }
  var successful=rows.Count(x=>x.ResultCode==ResultCodes.Success);return new(batch,ids.Length,successful,ids.Length-successful,rows);
 }
 private static long Valid(long id){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));return id;}
 private static string Required(string?m){if(string.IsNullOrWhiteSpace(m))throw new ArgumentException("Remarks are required for return and reject.");Length(m);return m.Trim();}
 private static void Length(string?m){if(m?.Length>2000)throw new ArgumentException("Remarks cannot exceed 2000 characters.");}
}
