namespace JamunaBank.Procurement.API.Services.Interfaces;

public interface ICurrentUserService
{
    string EmployeeId { get; }
    string Email { get; }
    string FullName { get; }
    long OrgUnitId { get; }
    string OrgUnitCode { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsInRole(string role);
}
