namespace JamunaBank.Procurement.API.Services.Interfaces;

public interface IDatabaseHealthService
{
    Task<int> CountActiveOrganizationUnitsAsync(CancellationToken cancellationToken);
}
