using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JamunaBank.Procurement.API.Authentication;

public sealed class ProcurementJwtAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> schemeOptions,
    ILoggerFactory logger,
    UrlEncoder encoder,
    JwtOptions jwtOptions)
    : AuthenticationHandler<AuthenticationSchemeOptions>(schemeOptions, logger, encoder)
{
    public const string SchemeName = "ProcurementJwt";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();
        if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.NoResult());

        var token = authorization["Bearer ".Length..].Trim();
        if (string.IsNullOrWhiteSpace(token))
            return Task.FromResult(AuthenticateResult.Fail("Bearer token is missing."));

        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true, ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true, ValidAudience = jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                ValidateLifetime = true, ClockSkew = TimeSpan.FromMinutes(1),
                NameClaimType = "name", RoleClaimType = "roles"
            };
            var tokenHandler = new JwtSecurityTokenHandler { MapInboundClaims = false };
            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName)));
        }
        catch (SecurityTokenException exception)
        {
            return Task.FromResult(AuthenticateResult.Fail(exception));
        }
        catch (ArgumentException exception)
        {
            return Task.FromResult(AuthenticateResult.Fail(exception));
        }
    }
}
