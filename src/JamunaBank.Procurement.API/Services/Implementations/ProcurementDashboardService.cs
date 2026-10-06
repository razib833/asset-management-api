using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Repositories.Interfaces;using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class ProcurementDashboardService(IProcurementDashboardRepository repository,IRequisitionMakerRepository requisitions):IProcurementDashboardService
{
 public Task<ProcurementDashboardDto>GetAsync(int year,CancellationToken ct){if(year<2000||year>2100)throw new ArgumentOutOfRangeException(nameof(year),"Year must be between 2000 and 2100.");return repository.GetAsync(year,ct);}
 public Task<IReadOnlyList<BranchRequisitionDto>>GetRequisitionsAsync(string?no,long?assetId,string?type,string?status,DateOnly?from,DateOnly?to,CancellationToken ct){if(assetId is<=0)throw new ArgumentOutOfRangeException(nameof(assetId));if(from.HasValue&&to.HasValue&&from>to)throw new ArgumentException("From date cannot be after to date.");return requisitions.GetAllAsync(no,assetId,type,status,from,to,ct);}
 public async Task<RequisitionDetailDto?>GetRequisitionAsync(long id,CancellationToken ct){if(id<=0)throw new ArgumentOutOfRangeException(nameof(id));var requester=await requisitions.GetRequesterAsync(id,ct);return requester is null?null:await requisitions.GetAsync(id,requester,ct);}
}
