using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IConcernDivisionSetupRepository
{
 Task<IReadOnlyList<ConcernDivisionDto>> GetDivisionsAsync(bool activeOnly,CancellationToken ct);
 Task<IReadOnlyList<ConcernOfficialDto>> GetOfficialsAsync(long divisionId,bool activeOnly,CancellationToken ct);
 Task<IReadOnlyList<ConcernAuthorityDto>> GetAuthoritiesAsync(long divisionId,bool activeOnly,CancellationToken ct);
 Task<StoredProcedureResult?> AddOfficialAsync(long divisionId,AddConcernOfficialRequest request,string actor,CancellationToken ct);
 Task<StoredProcedureResult?> DeactivateOfficialAsync(long mappingId,string actor,CancellationToken ct);
 Task<StoredProcedureResult?> AddAuthorityAsync(long divisionId,AddConcernAuthorityRequest request,string actor,CancellationToken ct);
 Task<StoredProcedureResult?> DeactivateAuthorityAsync(long mappingId,string actor,CancellationToken ct);
}
