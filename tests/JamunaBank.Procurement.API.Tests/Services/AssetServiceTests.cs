using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Implementations;
using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Tests.Services;
public static class AssetServiceTests
{
    public static void SearchFiltersAreApplied()
    {
        var repo=new FakeAssetRepository();var service=new AssetService(repo,new FakeCurrentUser());
        var rows=service.SearchAsync(7,"lap","LT-",true,CancellationToken.None).GetAwaiter().GetResult();
        TestAssert.Equal(1,rows.Count);TestAssert.Equal(1L,rows[0].Id);TestAssert.Equal(7L,repo.LastCategory!.Value);TestAssert.True(repo.LastActiveOnly);
    }
    public static void ActorComesFromCurrentUser()
    {
        var repo=new FakeAssetRepository();var service=new AssetService(repo,new FakeCurrentUser());
        service.CreateAsync(ValidCreate(),CancellationToken.None).GetAwaiter().GetResult();
        service.UpdateAsync(1,ValidUpdate(),CancellationToken.None).GetAwaiter().GetResult();
        TestAssert.Equal("EMP-ADMIN",repo.LastActor!);
    }
    public static void RequiredFieldsAreValidated()
    {
        var service=new AssetService(new FakeAssetRepository(),new FakeCurrentUser());
        TestAssert.Throws<ArgumentException>(()=>service.CreateAsync(new CreateAssetRequest(),CancellationToken.None).GetAwaiter().GetResult());
        TestAssert.Throws<ArgumentException>(()=>service.CreateAsync(ValidCreate(" "),CancellationToken.None).GetAwaiter().GetResult());
        TestAssert.Throws<ArgumentException>(()=>service.UpdateAsync(1,new UpdateAssetRequest(),CancellationToken.None).GetAwaiter().GetResult());
    }
    public static void ActivationPreservesStoredValues()
    {
        var repo=new FakeAssetRepository();var service=new AssetService(repo,new FakeCurrentUser());
        service.SetActiveAsync(1,false,CancellationToken.None).GetAwaiter().GetResult();
        TestAssert.Equal("Laptop",repo.LastUpdate!.AssetName);TestAssert.Equal("Each",repo.LastUpdate.UnitOfMeasurement);TestAssert.False(repo.LastUpdate.IsActive);
    }
    private static CreateAssetRequest ValidCreate(string code="LT-01")=>new(){AssetCategoryId=7,AssetCode=code,AssetName="Laptop",UnitOfMeasurement="Each"};
    private static UpdateAssetRequest ValidUpdate()=>new(){AssetCategoryId=7,AssetCode="LT-01",AssetName="Laptop",UnitOfMeasurement="Each",Criticality="MEDIUM"};
    private sealed class FakeCurrentUser:ICurrentUserService
    { public string EmployeeId=>"EMP-ADMIN";public string Email=>"a@b.com";public string FullName=>"Admin";public long OrgUnitId=>1;public string OrgUnitCode=>"ICT";public IReadOnlyCollection<string> Roles=>["ADMIN"];public bool IsInRole(string role)=>role=="ADMIN"; }
    private sealed class FakeAssetRepository:IAssetRepository
    {
        public long? LastCategory;public bool LastActiveOnly;public string? LastActor;public UpdateAssetRequest? LastUpdate;
        private readonly AssetDto[] data=[new(1,7,"Computers","LT-01","Laptop",null,"Each",null,null,null,"MEDIUM",true,true,true),new(2,7,"Computers","DT-01","Desktop",null,"Each",null,null,null,"MEDIUM",true,true,true)];
        public Task<IReadOnlyList<AssetDto>> GetAllAsync(long? categoryId,bool activeOnly,CancellationToken ct){LastCategory=categoryId;LastActiveOnly=activeOnly;return Task.FromResult<IReadOnlyList<AssetDto>>(data);}
        public Task<AssetDto?> GetByIdAsync(long id,CancellationToken ct)=>Task.FromResult<AssetDto?>(data.SingleOrDefault(x=>x.Id==id));
        public Task<StoredProcedureResult?> CreateAsync(CreateAssetRequest request,string actor,CancellationToken ct){LastActor=actor;return Task.FromResult<StoredProcedureResult?>(new("000","Success",1,null));}
        public Task<StoredProcedureResult?> UpdateAsync(long id,UpdateAssetRequest request,string actor,CancellationToken ct){LastActor=actor;LastUpdate=request;return Task.FromResult<StoredProcedureResult?>(new("000","Success",id,null));}
    }
}
