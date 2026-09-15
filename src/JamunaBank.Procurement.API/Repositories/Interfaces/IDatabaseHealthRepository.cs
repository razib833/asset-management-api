namespace JamunaBank.Procurement.API.Repositories.Interfaces;

public interface IDatabaseHealthRepository
{
    Task<int> CountActiveOrganizationUnitsAsync(CancellationToken cancellationToken);
}
