namespace JamunaBank.Procurement.API.DTOs;

public sealed record CommonLookupDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    int DisplayOrder);
