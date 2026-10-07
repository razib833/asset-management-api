using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IAssetSpecificationService
{
 Task<AssetSpecificationSetDto> GetAsync(long assetId,CancellationToken ct);
 Task<StoredProcedureResult?> CreateAsync(long assetId,SaveAssetSpecificationRequest request,CancellationToken ct);
 Task<StoredProcedureResult?> UpdateAsync(long specId,SaveAssetSpecificationRequest request,CancellationToken ct);
 Task<StoredProcedureResult?> CreateOptionAsync(long specId,CreateAssetSpecificationOptionRequest request,CancellationToken ct);
 Task<IReadOnlyList<StoredProcedureResult?>> UpdateOptionsAsync(long specId,IReadOnlyList<UpdateAssetSpecificationOptionRequest> requests,CancellationToken ct);
}
