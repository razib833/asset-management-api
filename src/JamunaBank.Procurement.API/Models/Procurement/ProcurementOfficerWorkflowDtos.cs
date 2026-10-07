using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ProcurementOfficerRequisitionDto(long Id,string Number,DateTime RequestedDate,decimal TotalEstimatedAmount,string Status,int ItemCount,string? AssignedToEmployeeId,bool IsOwnedByCurrentUser,string AssetNames,string OrgUnitName,string RequestedByEmployeeId,string RequestedByName,string RequisitionTypes);
public sealed class ProcurementOfficerActionRequest{[StringLength(2000)]public string? Remarks{get;init;}}
public sealed class ProcurementOfficerCommentRequest{public long? RequisitionItemId{get;init;}[Required,StringLength(4000)]public string CommentText{get;init;}=string.Empty;}
