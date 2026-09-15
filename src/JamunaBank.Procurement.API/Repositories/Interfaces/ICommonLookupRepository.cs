using JamunaBank.Procurement.API.DTOs;

namespace JamunaBank.Procurement.API.Repositories.Interfaces;

public interface ICommonLookupRepository
{
    Task<IReadOnlyList<CommonLookupDto>> GetByCategoryAsync(string category, CancellationToken cancellationToken);
}
