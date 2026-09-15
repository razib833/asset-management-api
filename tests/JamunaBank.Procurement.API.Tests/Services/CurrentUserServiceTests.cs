using System.Security.Claims;
using JamunaBank.Procurement.API.Services.Implementations;
using Microsoft.AspNetCore.Http;

namespace JamunaBank.Procurement.API.Tests.Services;

public static class CurrentUserServiceTests
{
    public static void ValidClaimsExposeAuthenticatedUser()
    {
        var service = CreateService();

        TestAssert.Equal("EMP-100", service.EmployeeId);
        TestAssert.Equal("user@jamunabank.com", service.Email);
        TestAssert.Equal("Test User", service.FullName);
        TestAssert.Equal(42L, service.OrgUnitId);
        TestAssert.Equal("ICT", service.OrgUnitCode);
        TestAssert.SequenceEqual(["MAKER", "MANAGER"], service.Roles);
    }

    public static void MissingRequiredClaimsThrow()
    {
        foreach (var missingClaim in new[] { "employee_id", "email", "name", "org_unit_id", "org_unit_code" })
        {
            var service = CreateService(missingClaim);
            var exception = TestAssert.Throws<InvalidOperationException>(() => ReadClaim(service, missingClaim));
            TestAssert.True(exception.Message.Contains(missingClaim, StringComparison.Ordinal));
        }
    }

    public static void RoleChecksAreCaseInsensitiveAndRejectMissingRole()
    {
        var service = CreateService();

        TestAssert.True(service.IsInRole("maker"));
        TestAssert.True(service.IsInRole("MANAGER"));
        TestAssert.False(service.IsInRole("ADMIN"));
        TestAssert.False(service.IsInRole(string.Empty));
    }

    public static void UnauthenticatedUserThrows()
    {
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
        };

        var service = new CurrentUserService(accessor);

        TestAssert.Throws<UnauthorizedAccessException>(() => _ = service.EmployeeId);
    }

    public static void OrganizationContextComesFromValidatedClaims()
    {
        var service = CreateService();

        TestAssert.Equal(42L, service.OrgUnitId);
        TestAssert.Equal("ICT", service.OrgUnitCode);
    }

    public static void InvalidOrganizationIdsThrow()
    {
        foreach (var value in new[] { "0", "not-a-number" })
        {
            var service = CreateService(orgUnitId: value);
            TestAssert.Throws<InvalidOperationException>(() => _ = service.OrgUnitId);
        }
    }

    private static CurrentUserService CreateService(string? excludedClaim = null, string orgUnitId = "42")
    {
        var claims = new[]
        {
            new Claim("employee_id", "EMP-100"),
            new Claim("email", "user@jamunabank.com"),
            new Claim("name", "Test User"),
            new Claim("org_unit_id", orgUnitId),
            new Claim("org_unit_code", "ICT"),
            new Claim("roles", "MAKER"),
            new Claim("roles", "MANAGER")
        }.Where(claim => claim.Type != excludedClaim);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test", "name", "roles"));
        var context = new DefaultHttpContext { User = principal };
        return new CurrentUserService(new HttpContextAccessor { HttpContext = context });
    }

    private static object ReadClaim(CurrentUserService service, string claimType) => claimType switch
    {
        "employee_id" => service.EmployeeId,
        "email" => service.Email,
        "name" => service.FullName,
        "org_unit_id" => service.OrgUnitId,
        "org_unit_code" => service.OrgUnitCode,
        _ => throw new ArgumentOutOfRangeException(nameof(claimType))
    };
}
