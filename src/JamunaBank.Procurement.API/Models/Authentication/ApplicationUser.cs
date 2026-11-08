namespace JamunaBank.Procurement.API.Models;

public sealed record ApplicationUser(
    string EmployeeId,
    string FullName,
    string Email,
    string Designation,
    long OrgUnitId,
    string OrgUnitCode,
    string OrgUnitName,
    string OrgUnitType,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions);
