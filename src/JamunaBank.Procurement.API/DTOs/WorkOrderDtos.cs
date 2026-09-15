using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed class CreateWorkOrderRequest{[Required,MinLength(1)]public IReadOnlyCollection<long> RequisitionIds{get;init;}=[];[Required,StringLength(60)]public string WorkOrderNo{get;init;}=string.Empty;public DateOnly WorkOrderDate{get;init;}[Required,StringLength(300)]public string SupplierVendor{get;init;}=string.Empty;[StringLength(100)]public string? ContractAgreementNo{get;init;}public decimal WorkOrderValue{get;init;}public DateOnly? DeliveryDate{get;init;}[StringLength(2000)]public string? Remarks{get;init;}}
public sealed record WorkOrderRequisitionDto(long RequisitionId,string RequisitionNo,decimal TotalEstimatedAmount,string ProcurementStatusCode);
public sealed record WorkOrderDto(long Id,string WorkOrderNo,DateOnly WorkOrderDate,string SupplierVendor,string? ContractAgreementNo,decimal WorkOrderValue,DateOnly? DeliveryDate,string? Remarks,IReadOnlyList<WorkOrderRequisitionDto> Requisitions);
public sealed record WorkOrderAssignmentResult(long RequisitionId,string? RequisitionNo,string ResultCode,string ResultMessage);
public sealed record WorkOrderBatchResult(Guid BatchReferenceId,long? WorkOrderId,string? WorkOrderNo,int TotalSelected,int SuccessfulCount,int FailedCount,IReadOnlyList<WorkOrderAssignmentResult> Results);
