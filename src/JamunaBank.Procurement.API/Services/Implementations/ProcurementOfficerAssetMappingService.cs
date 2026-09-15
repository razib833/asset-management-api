using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Repositories.Interfaces;using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class ProcurementOfficerAssetMappingService(IProcurementOfficerAssetMappingRepository repository,IDevelopmentEmployeeResolver employees,ICurrentUserService currentUser):IProcurementOfficerAssetMappingService
{
 public Task<IReadOnlyList<ProcurementOfficerAssetMappingDto>> GetByAssetAsync(long id,bool activeOnly,CancellationToken ct){Positive(id);return repository.GetByAssetAsync(id,activeOnly,ct);}
 public Task<StoredProcedureResult?> AddAsync(long id,AddProcurementOfficerAssetMappingRequest x,CancellationToken ct){Positive(id);if(string.IsNullOrWhiteSpace(x.EmployeeId))throw new ArgumentException("Employee ID is required.");if(employees.FindByEmployeeId(x.EmployeeId.Trim()) is null)throw new ArgumentException($"Employee '{x.EmployeeId.Trim()}' is not a configured development user.");if(x.EffectiveTo<x.EffectiveFrom)throw new ArgumentException("Effective-to date cannot be earlier than effective-from date.");return repository.AddAsync(id,x,currentUser.EmployeeId,ct);}
 public Task<StoredProcedureResult?> DeactivateAsync(long assetId,long id,CancellationToken ct){Positive(assetId);Positive(id);return repository.DeactivateAsync(assetId,id,currentUser.EmployeeId,ct);}
 private static void Positive(long id){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));}
}
