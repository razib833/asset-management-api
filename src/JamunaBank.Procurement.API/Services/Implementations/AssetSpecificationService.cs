using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Repositories.Interfaces;using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class AssetSpecificationService(IAssetSpecificationRepository repository,ICurrentUserService currentUser):IAssetSpecificationService
{
 private static readonly HashSet<string> Types=new(["TEXT","NUMBER","DATE","DROPDOWN","MULTI_SELECT","YES_NO"],StringComparer.OrdinalIgnoreCase);
 public Task<AssetSpecificationSetDto> GetAsync(long id,CancellationToken ct){Positive(id);return repository.GetByAssetAsync(id,ct);}
 public Task<StoredProcedureResult?> CreateAsync(long id,SaveAssetSpecificationRequest x,CancellationToken ct){Positive(id);Validate(x);return repository.CreateAsync(id,x,currentUser.EmployeeId,ct);}
 public Task<StoredProcedureResult?> UpdateAsync(long id,SaveAssetSpecificationRequest x,CancellationToken ct){Positive(id);Validate(x);return repository.UpdateAsync(id,x,currentUser.EmployeeId,ct);}
 public Task<StoredProcedureResult?> CreateOptionAsync(long id,CreateAssetSpecificationOptionRequest x,CancellationToken ct){Positive(id);ValidateOption(x.Value,x.DisplayOrder);return repository.CreateOptionAsync(id,x,currentUser.EmployeeId,ct);}
 public async Task<IReadOnlyList<StoredProcedureResult?>> UpdateOptionsAsync(long id,IReadOnlyList<UpdateAssetSpecificationOptionRequest> xs,CancellationToken ct){Positive(id);if(xs.Count==0)throw new ArgumentException("At least one option is required.");var results=new List<StoredProcedureResult?>();foreach(var x in xs){Positive(x.OptionId);ValidateOption(x.Value,x.DisplayOrder);results.Add(await repository.UpdateOptionAsync(id,x,currentUser.EmployeeId,ct));}return results;}
 private static void Validate(SaveAssetSpecificationRequest x){if(string.IsNullOrWhiteSpace(x.Name))throw new ArgumentException("Specification name is required.");if(!Types.Contains(x.Type))throw new ArgumentException("Specification type is invalid.");if(x.DisplayOrder<0)throw new ArgumentException("Display order cannot be negative.");}
 private static void ValidateOption(string value,int order){if(string.IsNullOrWhiteSpace(value))throw new ArgumentException("Option value is required.");if(order<0)throw new ArgumentException("Display order cannot be negative.");}
 private static void Positive(long id){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));}
}
