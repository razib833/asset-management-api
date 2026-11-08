using JamunaBank.Procurement.API.DTOs;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IProcurementDashboardRepository{Task<ProcurementDashboardDto>GetAsync(int year,CancellationToken ct);}
