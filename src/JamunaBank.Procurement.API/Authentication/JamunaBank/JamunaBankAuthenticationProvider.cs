using JamunaBank.Procurement.API.Authentication.Interfaces;
using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Authentication.JamunaBank;

public sealed class JamunaBankAuthenticationProvider(
    AuthenticationOptions options,
    ILogger<JamunaBankAuthenticationProvider> logger) : IAuthenticationProvider
{
    public Task<ApplicationUser?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        _ = email;
        _ = password;
        _ = cancellationToken;
        logger.LogWarning("Jamuna Bank authentication was selected but the external API contract is not configured.");

        // TODO: Configure options.JamunaBank.BaseUrl, AuthenticationEndpoint and TimeoutSeconds.
        // TODO: Call the official User Management API after its request/response contract is supplied.
        // TODO: Map only trusted API identity, organization, role and permission fields to ApplicationUser.
        // No external contract is invented at this stage.
        throw new NotSupportedException(
            $"Jamuna Bank authentication is not implemented. Configured endpoint: '{options.JamunaBank.AuthenticationEndpoint}'.");
    }
}
