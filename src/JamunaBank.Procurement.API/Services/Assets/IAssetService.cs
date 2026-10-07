using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IAssetService
{
    Task<IReadOnlyList<AssetDto>> SearchAsync(long? categoryId,string? name,string? code,bool? isActive,CancellationToken ct);
    Task<AssetDto?> GetByIdAsync(long id,CancellationToken ct);
    Task<StoredProcedureResult?> CreateAsync(CreateAssetRequest request,CancellationToken ct);
    Task<StoredProcedureResult?> UpdateAsync(long id,UpdateAssetRequest request,CancellationToken ct);
    Task<StoredProcedureResult?> SetActiveAsync(long id,bool active,CancellationToken ct);
}
