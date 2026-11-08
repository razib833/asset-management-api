namespace JamunaBank.Procurement.API.DTOs;

public sealed record SystemVersionDto(
    string Version,
    string? ReleaseName,
    DateTime AppliedOn,
    string DatabaseName);
