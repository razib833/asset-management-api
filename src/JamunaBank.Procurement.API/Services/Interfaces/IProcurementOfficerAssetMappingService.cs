using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IProcurementOfficerAssetMappingService
{
 Task<IReadOnlyList<ProcurementOfficerAssetMappingDto>> GetByAssetAsync(long assetId,bool activeOnly,CancellationToken ct);
 Task<StoredProcedureResult?> AddAsync(long assetId,AddProcurementOfficerAssetMappingRequest request,CancellationToken ct);
 Task<StoredProcedureResult?> DeactivateAsync(long assetId,long mappingId,CancellationToken ct);
}
