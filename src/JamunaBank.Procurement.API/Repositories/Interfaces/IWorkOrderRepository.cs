using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IWorkOrderRepository{Task<StoredProcedureResult?>CreateAsync(CreateWorkOrderRequest request,string employeeId,CancellationToken ct);Task<WorkOrderDto?>GetAsync(long id,CancellationToken ct);Task<IReadOnlyList<WorkOrderDto>>GetByRequisitionAsync(long requisitionId,CancellationToken ct);}
