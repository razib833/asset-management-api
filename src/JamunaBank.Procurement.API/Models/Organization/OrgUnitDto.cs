namespace JamunaBank.Procurement.API.DTOs;

public sealed record OrgUnitDto(
    long Id,
    long? ParentId,
    string Code,
    string Name,
    string Type,
    bool IsActive);
