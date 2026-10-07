using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Services.Interfaces;

public interface IOrgUnitService
{
    Task<PagedResult<OrgUnitDto>> GetAsync(string? type, int page, int pageSize, CancellationToken cancellationToken);
    Task<OrgUnitDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
}
