using JamunaBank.Procurement.API.DTOs;

namespace JamunaBank.Procurement.API.Services.Interfaces;

public interface IDatabaseHealthService
{
    Task<int> CountActiveOrganizationUnitsAsync(CancellationToken cancellationToken);
    Task<SystemVersionDto?> GetSystemVersionAsync(CancellationToken cancellationToken);
}
