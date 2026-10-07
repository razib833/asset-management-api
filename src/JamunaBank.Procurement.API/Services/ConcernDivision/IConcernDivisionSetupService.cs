using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IConcernDivisionSetupService
{
 Task<IReadOnlyList<ConcernDivisionDto>> GetDivisionsAsync(bool activeOnly,CancellationToken ct);
 Task<IReadOnlyList<ConcernOfficialDto>> GetOfficialsAsync(long divisionId,bool activeOnly,CancellationToken ct);
 Task<IReadOnlyList<ConcernAuthorityDto>> GetAuthoritiesAsync(long divisionId,bool activeOnly,CancellationToken ct);
 Task<StoredProcedureResult?> AddOfficialAsync(long divisionId,AddConcernOfficialRequest request,CancellationToken ct);
 Task<StoredProcedureResult?> DeactivateOfficialAsync(long mappingId,CancellationToken ct);
 Task<StoredProcedureResult?> AddAuthorityAsync(long divisionId,AddConcernAuthorityRequest request,CancellationToken ct);
 Task<StoredProcedureResult?> DeactivateAuthorityAsync(long mappingId,CancellationToken ct);
}
