using JamunaBank.Procurement.API.DTOs;

namespace JamunaBank.Procurement.API.Repositories.Interfaces;

public interface IDatabaseHealthRepository
{
    Task<int> CountActiveOrganizationUnitsAsync(CancellationToken cancellationToken);
    Task<SystemVersionDto?> GetSystemVersionAsync(CancellationToken cancellationToken);
}
