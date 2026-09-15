using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Repositories.Interfaces;using JamunaBank.Procurement.API.Services.Implementations;using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Tests.Services;
public static class ConcernAuthorityServiceTests
{
 public static void QueueAndActionsUseAuthenticatedAuthority(){var s=New(out var r);s.GetPendingAsync(default).GetAwaiter().GetResult();s.ApproveAsync(1,null,default).GetAwaiter().GetResult();s.ReturnAsync(1,"clarify",default).GetAwaiter().GetResult();TestAssert.Equal("CA-1",r.Employee!);TestAssert.Equal("return",r.Action!);}
 public static void ReturnAndRejectRequireRemarks(){var s=New(out _);TestAssert.Throws<ArgumentException>(()=>s.ReturnAsync(1," ",default).GetAwaiter().GetResult());TestAssert.Throws<ArgumentException>(()=>s.RejectAsync(1,null,default).GetAwaiter().GetResult());}
 public static void BulkUsesSharedBatchAndKeepsPartialResults(){var s=New(out var r);r.Results[2]=new("004","Already acted",2,null);var x=s.BulkApproveAsync(new(){RequisitionIds=[1,2,3],Remarks="ok"},default).GetAwaiter().GetResult();TestAssert.Equal(2,x.SuccessfulCount);TestAssert.Equal(1,x.FailedCount);TestAssert.True(r.Batches.All(b=>b==x.BatchReferenceId));TestAssert.Equal("REQ-2",x.Results[1].RequisitionNo!);}
 public static void ApprovalDestinationComesFromProcedureResult(){var s=New(out var r);r.Results[1]=new("000","Success",1,"PROCUREMENT_AUTHORITY");var x=s.BulkApproveAsync(new(){RequisitionIds=[1]},default).GetAwaiter().GetResult();TestAssert.Equal("PROCUREMENT_AUTHORITY",x.Results[0].NextDestination!);}
 private static ConcernAuthorityService New(out Repo r){r=new();return new(r,new User());}
 private sealed class User:ICurrentUserService{public string EmployeeId=>"CA-1";public string Email=>"";public string FullName=>"";public long OrgUnitId=>9;public string OrgUnitCode=>"DIV";public IReadOnlyCollection<string>Roles=>["CONCERN_AUTHORITY"];public bool IsInRole(string x)=>x=="CONCERN_AUTHORITY";}
 private sealed class Repo:IConcernAuthorityRepository
 {
  public string?Employee;public string?Action;public Dictionary<long,StoredProcedureResult>Results=new();public List<Guid?>Batches=[];
  private Task<StoredProcedureResult?>R(string a,string e,long id){Employee=e;Action=a;return Task.FromResult<StoredProcedureResult?>(Results.GetValueOrDefault(id,new("000","Success",id,"READY_FOR_PROCUREMENT")));}
  public Task<IReadOnlyList<ConcernAuthorityPendingDto>>GetPendingAsync(string e,CancellationToken c){Employee=e;return Task.FromResult<IReadOnlyList<ConcernAuthorityPendingDto>>([new(1,"REQ-1",DateTime.UtcNow,10,1,"IT","Maker",1),new(2,"REQ-2",DateTime.UtcNow,20,1,"IT","Maker",1),new(3,"REQ-3",DateTime.UtcNow,30,1,"IT","Maker",1)]);}
  public Task<RequisitionDetailDto?>GetDetailAsync(long id,string e,CancellationToken c)=>Task.FromResult<RequisitionDetailDto?>(null);
  public Task<ConcernAuthorityReviewContextDto?>GetContextAsync(long id,string e,CancellationToken c)=>Task.FromResult<ConcernAuthorityReviewContextDto?>(null);
  public Task<StoredProcedureResult?>ApproveAsync(long id,string e,string?m,Guid?b,CancellationToken c){Batches.Add(b);return R("approve",e,id);}
  public Task<StoredProcedureResult?>ReturnAsync(long id,string e,string m,CancellationToken c)=>R("return",e,id);
  public Task<StoredProcedureResult?>RejectAsync(long id,string e,string m,Guid?b,CancellationToken c){Batches.Add(b);return R("reject",e,id);}
 }
}
