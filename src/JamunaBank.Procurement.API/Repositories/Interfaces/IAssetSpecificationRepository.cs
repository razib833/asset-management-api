using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IAssetSpecificationRepository
{
 Task<AssetSpecificationSetDto> GetByAssetAsync(long assetId,CancellationToken ct);
 Task<StoredProcedureResult?> CreateAsync(long assetId,SaveAssetSpecificationRequest request,string actor,CancellationToken ct);
 Task<StoredProcedureResult?> UpdateAsync(long specId,SaveAssetSpecificationRequest request,string actor,CancellationToken ct);
 Task<StoredProcedureResult?> CreateOptionAsync(long specId,CreateAssetSpecificationOptionRequest request,string actor,CancellationToken ct);
 Task<StoredProcedureResult?> UpdateOptionAsync(long specId,UpdateAssetSpecificationOptionRequest request,string actor,CancellationToken ct);
}
