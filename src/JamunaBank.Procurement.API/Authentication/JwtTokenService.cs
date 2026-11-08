using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Responses;
using Microsoft.IdentityModel.Tokens;

namespace JamunaBank.Procurement.API.Authentication;

public sealed class JwtTokenService(JwtOptions options, TimeProvider timeProvider)
{
    public LoginResponse CreateToken(ApplicationUser user)
    {
        var now = timeProvider.GetUtcNow();
        var expiresAt = now.AddMinutes(options.ExpiryMinutes);
        var claims = new List<Claim>
        {
            new("employee_id", user.EmployeeId), new("email", user.Email), new("name", user.FullName),
            new("designation", user.Designation), new("org_unit_id", user.OrgUnitId.ToString(CultureInfo.InvariantCulture)),
            new("org_unit_code", user.OrgUnitCode), new("org_unit_name", user.OrgUnitName), new("org_unit_type", user.OrgUnitType)
        };
        claims.AddRange(user.Roles.Select(role => new Claim("roles", role)));
        claims.AddRange(user.Permissions.Select(permission => new Claim("permissions", permission)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey));
        var token = new JwtSecurityToken(options.Issuer, options.Audience, claims, now.UtcDateTime, expiresAt.UtcDateTime,
            new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, user);
    }
}
