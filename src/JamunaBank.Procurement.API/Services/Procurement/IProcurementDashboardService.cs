using JamunaBank.Procurement.API.DTOs;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IProcurementDashboardService{Task<ProcurementDashboardDto>GetAsync(int year,CancellationToken ct);Task<IReadOnlyList<BranchRequisitionDto>>GetRequisitionsAsync(string?no,long?assetId,string?type,string?status,DateOnly?from,DateOnly?to,CancellationToken ct);Task<RequisitionDetailDto?>GetRequisitionAsync(long id,CancellationToken ct);}
