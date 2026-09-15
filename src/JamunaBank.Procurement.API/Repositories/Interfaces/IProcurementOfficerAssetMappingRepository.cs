using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IProcurementOfficerAssetMappingRepository
{
 Task<IReadOnlyList<ProcurementOfficerAssetMappingDto>> GetByAssetAsync(long assetId,bool activeOnly,CancellationToken ct);
 Task<StoredProcedureResult?> AddAsync(long assetId,AddProcurementOfficerAssetMappingRequest request,string actor,CancellationToken ct);
 Task<StoredProcedureResult?> DeactivateAsync(long assetId,long mappingId,string actor,CancellationToken ct);
}
