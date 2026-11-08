using System.Globalization;
using System.Security.Claims;
using JamunaBank.Procurement.API.Services.Interfaces;

namespace JamunaBank.Procurement.API.Services.Implementations;

public sealed class CurrentUserService : ICurrentUserService
{
    private const string EmployeeIdClaim = "employee_id";
    private const string EmailClaim = "email";
    private const string FullNameClaim = "name";
    private const string OrgUnitIdClaim = "org_unit_id";
    private const string OrgUnitCodeClaim = "org_unit_code";
    private const string RoleClaim = "roles";
    private readonly ClaimsPrincipal? _principal;
    private readonly Lazy<IReadOnlyCollection<string>> _roles;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _principal = httpContextAccessor.HttpContext?.User;

        _roles = new Lazy<IReadOnlyCollection<string>>(() => AuthenticatedPrincipal().FindAll(RoleClaim)
            .Select(claim => claim.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray());
    }

    public string EmployeeId => RequiredClaim(EmployeeIdClaim);
    public string Email => RequiredClaim(EmailClaim);
    public string FullName => RequiredClaim(FullNameClaim);
    public long OrgUnitId => ParseOrgUnitId(RequiredClaim(OrgUnitIdClaim));
    public string OrgUnitCode => RequiredClaim(OrgUnitCodeClaim);
    public IReadOnlyCollection<string> Roles => _roles.Value;

    public bool IsInRole(string role) =>
        !string.IsNullOrWhiteSpace(role) && Roles.Contains(role, StringComparer.OrdinalIgnoreCase);

    private string RequiredClaim(string claimType)
    {
        var value = AuthenticatedPrincipal().FindFirstValue(claimType);
        return !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new InvalidOperationException($"Authenticated user is missing required claim '{claimType}'.");
    }

    private ClaimsPrincipal AuthenticatedPrincipal() =>
        _principal?.Identity?.IsAuthenticated == true
            ? _principal
            : throw new UnauthorizedAccessException("An authenticated user is required.");

    private static long ParseOrgUnitId(string value) =>
        long.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var orgUnitId) && orgUnitId > 0
            ? orgUnitId
            : throw new InvalidOperationException("Authenticated user claim 'org_unit_id' must be a positive integer.");
}
