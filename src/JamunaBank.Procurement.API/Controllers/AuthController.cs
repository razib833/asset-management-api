using JamunaBank.Procurement.API.Authentication.Interfaces;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Requests;
using JamunaBank.Procurement.API.Responses;
using JamunaBank.Procurement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JamunaBank.Procurement.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthenticationService authentication, ICurrentUserService currentUser) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var login = await authentication.LoginAsync(request, cancellationToken);
        if (login is null)
            return Unauthorized(ApiResponse<LoginResponse>.Fail("Authentication failed.", ["Invalid email or password."], HttpContext.TraceIdentifier));

        return Ok(ApiResponse<LoginResponse>.Ok(login, "Authentication succeeded.", HttpContext.TraceIdentifier));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<ApiResponse<object>> Me()
    {
        var user = new
        {
            currentUser.EmployeeId,
            currentUser.Email,
            currentUser.FullName,
            currentUser.OrgUnitId,
            currentUser.OrgUnitCode,
            currentUser.Roles
        };
        return Ok(ApiResponse<object>.Ok(user, "Authenticated user loaded.", HttpContext.TraceIdentifier));
    }
}
