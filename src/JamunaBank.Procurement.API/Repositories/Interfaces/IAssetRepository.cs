using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IAssetRepository
{
    Task<IReadOnlyList<AssetDto>> GetAllAsync(long? categoryId, bool activeOnly, CancellationToken cancellationToken);
    Task<AssetDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<StoredProcedureResult?> CreateAsync(CreateAssetRequest request, string actor, CancellationToken cancellationToken);
    Task<StoredProcedureResult?> UpdateAsync(long id, UpdateAssetRequest request, string actor, CancellationToken cancellationToken);
}
