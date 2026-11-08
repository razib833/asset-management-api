namespace JamunaBank.Procurement.API.DTOs;
public sealed record ReadyRequisitionDto(long RequisitionId,string RequisitionNo,DateTime RequisitionDate,string RequestingUnit,string AssetNames,string RequisitionTypes,long? ConcernDivisionId,string? ConcernDivisionName,DateTime ReadyDate);
public sealed record ConcernDivisionOptionDto(long Id,string Name);
public sealed record ReadyRequisitionPageDto(IReadOnlyList<ReadyRequisitionDto> Items,int PageNumber,int PageSize,int TotalCount,int TotalPages,IReadOnlyList<ConcernDivisionOptionDto> ConcernDivisions);
public sealed record RequisitionFormHeaderDto(long RequisitionId,string RequisitionNo,DateTime RequisitionDate,string RequestingUnit,string RequestedBy,string? Designation,string? ConcernDivisionName,string Justification,string CurrentStatus,DateTime GeneratedOn,string GeneratedBy,string? GeneratedByUnit);
public sealed record RequisitionFormItemDto(long ItemId,string AssetName,string RequisitionType,decimal Quantity,string PurposeJustification,string? AdditionalRequirement,string? Specifications,string? AssetTag,string? SerialNo,string? CurrentCondition,string? ReplacementAdditionalInfo);
public sealed record RequisitionFormHistoryDto(int Sequence,string Stage,string OfficialName,string? Designation,string? Unit,string Decision,string? Remarks,DateTime ActionDateTime);
public sealed record RequisitionFormStatusDto(string CurrentStatus,DateTime ReadyForProcurementOn);
public sealed record RequisitionFormDto(RequisitionFormHeaderDto Requisition,IReadOnlyList<RequisitionFormItemDto> Items,IReadOnlyList<RequisitionFormHistoryDto> WorkflowHistory,RequisitionFormStatusDto Status);
