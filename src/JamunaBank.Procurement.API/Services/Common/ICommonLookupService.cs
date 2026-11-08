using JamunaBank.Procurement.API.DTOs;

namespace JamunaBank.Procurement.API.Services.Interfaces;

public interface ICommonLookupService
{
    Task<IReadOnlyList<CommonLookupDto>> GetByCategoryAsync(string category, CancellationToken cancellationToken);
}
