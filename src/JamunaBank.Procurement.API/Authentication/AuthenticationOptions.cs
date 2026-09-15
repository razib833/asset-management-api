namespace JamunaBank.Procurement.API.Authentication;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";
    public string Mode { get; init; } = AuthenticationModes.Development;
    public List<DevelopmentUserConfiguration> DevelopmentUsers { get; init; } = [];
    public JamunaBankAuthenticationConfiguration JamunaBank { get; init; } = new();
}

public sealed class DevelopmentUserConfiguration
{
    public string EmployeeId { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Designation { get; init; } = string.Empty;
    public long OrgUnitId { get; init; }
    public string OrgUnitCode { get; init; } = string.Empty;
    public string OrgUnitName { get; init; } = string.Empty;
    public string OrgUnitType { get; init; } = string.Empty;
    public string[] Roles { get; init; } = [];
    public string[] Permissions { get; init; } = [];
}

public sealed class JamunaBankAuthenticationConfiguration
{
    public string BaseUrl { get; init; } = string.Empty;
    public string AuthenticationEndpoint { get; init; } = string.Empty;
    public int TimeoutSeconds { get; init; } = 30;
}

public static class AuthenticationModes
{
    public const string Development = "Development";
    public const string JamunaBank = "JamunaBank";
}
