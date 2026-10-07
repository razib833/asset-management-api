using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Repositories.Interfaces;using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class ConcernDivisionSetupService(IConcernDivisionSetupRepository repository,IDevelopmentEmployeeResolver employees,ICurrentUserService currentUser):IConcernDivisionSetupService
{
 public Task<IReadOnlyList<ConcernDivisionDto>> GetDivisionsAsync(bool activeOnly,CancellationToken ct)=>repository.GetDivisionsAsync(activeOnly,ct);
 public Task<IReadOnlyList<ConcernOfficialDto>> GetOfficialsAsync(long id,bool activeOnly,CancellationToken ct){Positive(id);return repository.GetOfficialsAsync(id,activeOnly,ct);}
 public Task<IReadOnlyList<ConcernAuthorityDto>> GetAuthoritiesAsync(long id,bool activeOnly,CancellationToken ct){Positive(id);return repository.GetAuthoritiesAsync(id,activeOnly,ct);}
 public Task<StoredProcedureResult?> AddOfficialAsync(long id,AddConcernOfficialRequest x,CancellationToken ct){Positive(id);ValidateEmployee(x.EmployeeId);Dates(x.EffectiveFrom,x.EffectiveTo);return repository.AddOfficialAsync(id,x,currentUser.EmployeeId,ct);}
 public Task<StoredProcedureResult?> DeactivateOfficialAsync(long id,CancellationToken ct){Positive(id);return repository.DeactivateOfficialAsync(id,currentUser.EmployeeId,ct);}
 public Task<StoredProcedureResult?> AddAuthorityAsync(long id,AddConcernAuthorityRequest x,CancellationToken ct){Positive(id);ValidateEmployee(x.EmployeeId);Dates(x.EffectiveFrom,x.EffectiveTo);if(x.AuthorityLevel<1)throw new ArgumentException("Authority level must be positive.");return repository.AddAuthorityAsync(id,x,currentUser.EmployeeId,ct);}
 public Task<StoredProcedureResult?> DeactivateAuthorityAsync(long id,CancellationToken ct){Positive(id);return repository.DeactivateAuthorityAsync(id,currentUser.EmployeeId,ct);}
 private void ValidateEmployee(string id){if(string.IsNullOrWhiteSpace(id))throw new ArgumentException("Employee ID is required.");if(employees.FindByEmployeeId(id.Trim()) is null)throw new ArgumentException($"Employee '{id.Trim()}' is not a configured development user.");}
 private static void Dates(DateOnly from,DateOnly? to){if(to<from)throw new ArgumentException("Effective-to date cannot be earlier than effective-from date.");}
 private static void Positive(long id){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));}
}
