using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;

namespace JamunaBank.Procurement.API.Services.Implementations;

public sealed class DatabaseHealthService(IDatabaseHealthRepository repository) : IDatabaseHealthService
{
    public Task<int> CountActiveOrganizationUnitsAsync(CancellationToken cancellationToken) =>
        repository.CountActiveOrganizationUnitsAsync(cancellationToken);
}
