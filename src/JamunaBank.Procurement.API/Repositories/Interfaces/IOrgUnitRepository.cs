using JamunaBank.Procurement.API.DTOs;

namespace JamunaBank.Procurement.API.Repositories.Interfaces;

public interface IOrgUnitRepository
{
    Task<IReadOnlyList<OrgUnitDto>> GetAllAsync(string? type, bool activeOnly, CancellationToken cancellationToken);
}
