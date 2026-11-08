using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ManagerPendingRequisitionDto(long Id,string Number,DateTime RequestedDate,string Justification,decimal TotalEstimatedAmount,long RequestedOrgUnitId,string RequestedByEmployeeId,string RequestedByName,string Status,int ItemCount);
public sealed class ManagerActionRequest{[StringLength(2000)]public string? Remarks{get;init;}}
