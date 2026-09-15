using System.Security.Cryptography;
using System.Text;
using JamunaBank.Procurement.API.Authentication.Interfaces;
using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Authentication.Development;

public sealed class DevelopmentAuthenticationProvider(AuthenticationOptions options) : IAuthenticationProvider
{
    public Task<ApplicationUser?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var configured = options.DevelopmentUsers.FirstOrDefault(user =>
            string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase));

        if (configured is null || !FixedTimeEquals(configured.Password, password))
        {
            return Task.FromResult<ApplicationUser?>(null);
        }

        ApplicationUser user = new(
            configured.EmployeeId, configured.FullName, configured.Email, configured.Designation,
            configured.OrgUnitId, configured.OrgUnitCode, configured.OrgUnitName, configured.OrgUnitType,
            configured.Roles, configured.Permissions);
        return Task.FromResult<ApplicationUser?>(user);
    }

    private static bool FixedTimeEquals(string expected, string supplied)
    {
        var expectedHash = SHA256.HashData(Encoding.UTF8.GetBytes(expected));
        var suppliedHash = SHA256.HashData(Encoding.UTF8.GetBytes(supplied));
        return CryptographicOperations.FixedTimeEquals(expectedHash, suppliedHash);
    }
}
