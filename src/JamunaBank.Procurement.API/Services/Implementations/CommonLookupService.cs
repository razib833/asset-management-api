using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;

namespace JamunaBank.Procurement.API.Services.Implementations;

public sealed class CommonLookupService(ICommonLookupRepository repository) : ICommonLookupService
{
    public Task<IReadOnlyList<CommonLookupDto>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        var normalized = category.Trim().ToUpperInvariant();
        if (normalized.Length > 50)
            throw new ArgumentException("Lookup category cannot exceed 50 characters.", nameof(category));
        return repository.GetByCategoryAsync(normalized, cancellationToken);
    }
}
