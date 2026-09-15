using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class AssetService(IAssetRepository repository, ICurrentUserService currentUser) : IAssetService
{
    private static readonly HashSet<string> Criticalities = new(["LOW","MEDIUM","HIGH","CRITICAL"],StringComparer.OrdinalIgnoreCase);
    public async Task<IReadOnlyList<AssetDto>> SearchAsync(long? categoryId,string? name,string? code,bool? isActive,CancellationToken ct)
    {
        if(categoryId<=0) throw new ArgumentOutOfRangeException(nameof(categoryId));
        var rows=await repository.GetAllAsync(categoryId,isActive==true,ct);
        return rows.Where(x=>(!isActive.HasValue||x.IsActive==isActive)&& (string.IsNullOrWhiteSpace(name)||x.Name.Contains(name.Trim(),StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrWhiteSpace(code)||x.Code.Contains(code.Trim(),StringComparison.OrdinalIgnoreCase))).ToArray();
    }
    public Task<AssetDto?> GetByIdAsync(long id,CancellationToken ct){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));return repository.GetByIdAsync(id,ct);}
    public Task<StoredProcedureResult?> CreateAsync(CreateAssetRequest request,CancellationToken ct){Validate(request);return repository.CreateAsync(request,currentUser.EmployeeId,ct);}
    public Task<StoredProcedureResult?> UpdateAsync(long id,UpdateAssetRequest request,CancellationToken ct){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));Validate(request);return repository.UpdateAsync(id,request,currentUser.EmployeeId,ct);}
    public async Task<StoredProcedureResult?> SetActiveAsync(long id,bool active,CancellationToken ct){var asset=await GetByIdAsync(id,ct);if(asset is null)return new StoredProcedureResult("002","Not found",id,null);return await UpdateAsync(id,new UpdateAssetRequest{AssetCategoryId=asset.AssetCategoryId,AssetCode=asset.Code,AssetName=asset.Name,Description=asset.Description,UnitOfMeasurement=asset.UnitOfMeasurement,DefaultWarranty=asset.DefaultWarranty,UsefulLife=asset.UsefulLife,CapitalizationThreshold=asset.CapitalizationThreshold,Criticality=asset.Criticality,IsInventoryItem=asset.IsInventoryItem,IsDepreciable=asset.IsDepreciable,IsActive=active},ct);}
    private static void Validate(CreateAssetRequest x){if(x.AssetCategoryId<=0)throw new ArgumentException("Category is required.");if(string.IsNullOrWhiteSpace(x.AssetCode))throw new ArgumentException("Asset code is required.");if(string.IsNullOrWhiteSpace(x.AssetName))throw new ArgumentException("Asset name is required.");if(string.IsNullOrWhiteSpace(x.UnitOfMeasurement))throw new ArgumentException("Unit of measurement is required.");if(!Criticalities.Contains(x.Criticality))throw new ArgumentException("Criticality is invalid.");}
    private static void Validate(UpdateAssetRequest x){if(x.AssetCategoryId<=0)throw new ArgumentException("Category is required.");if(string.IsNullOrWhiteSpace(x.AssetCode))throw new ArgumentException("Asset code is required.");if(string.IsNullOrWhiteSpace(x.AssetName))throw new ArgumentException("Asset name is required.");if(string.IsNullOrWhiteSpace(x.UnitOfMeasurement))throw new ArgumentException("Unit of measurement is required.");if(!Criticalities.Contains(x.Criticality))throw new ArgumentException("Criticality is invalid.");}
}
