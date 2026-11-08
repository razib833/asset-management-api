using JamunaBank.Procurement.API.Authentication.Interfaces;
using JamunaBank.Procurement.API.Requests;
using JamunaBank.Procurement.API.Responses;

namespace JamunaBank.Procurement.API.Authentication;

public sealed class AuthenticationService(IAuthenticationProvider provider, JwtTokenService tokens) : IAuthenticationService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await provider.AuthenticateAsync(request.Email, request.Password, cancellationToken);
        return user is null ? null : tokens.CreateToken(user);
    }
}
