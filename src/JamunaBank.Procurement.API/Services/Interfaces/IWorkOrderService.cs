using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IWorkOrderService{Task<StoredProcedureResult?>CreateAsync(CreateWorkOrderRequest request,CancellationToken ct);Task<WorkOrderBatchResult>BulkAssignAsync(CreateWorkOrderRequest request,CancellationToken ct);Task<WorkOrderDto?>GetAsync(long id,CancellationToken ct);Task<IReadOnlyList<WorkOrderDto>>GetByRequisitionAsync(long requisitionId,CancellationToken ct);}
