using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;

namespace JamunaBank.Procurement.API.Services.Implementations;

public sealed class OrgUnitService(IOrgUnitRepository repository) : IOrgUnitService
{
    private static readonly HashSet<string> SupportedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "BRANCH", "DIVISION"
    };

    public async Task<PagedResult<OrgUnitDto>> GetAsync(
        string? type,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        ValidatePaging(page, pageSize);
        var normalizedType = NormalizeType(type);
        var items = await repository.GetAllAsync(normalizedType, true, cancellationToken);
        var pageItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToArray();
        var totalPages = items.Count == 0 ? 0 : (int)Math.Ceiling(items.Count / (double)pageSize);
        return new PagedResult<OrgUnitDto>(pageItems, page, pageSize, items.Count, totalPages);
    }

    public async Task<OrgUnitDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Organization unit ID must be positive.");
        var items = await repository.GetAllAsync(null, true, cancellationToken);
        return items.SingleOrDefault(item => item.Id == id);
    }

    private static string? NormalizeType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type)) return null;
        var normalized = type.Trim().ToUpperInvariant();
        return SupportedTypes.Contains(normalized)
            ? normalized
            : throw new ArgumentException("Organization unit type must be Branch or Division.", nameof(type));
    }

    private static void ValidatePaging(int page, int pageSize)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page must be at least 1.");
        if (pageSize is < 1 or > 200)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 200.");
    }
}
