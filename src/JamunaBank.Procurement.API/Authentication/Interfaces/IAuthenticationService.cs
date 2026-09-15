using JamunaBank.Procurement.API.Requests;
using JamunaBank.Procurement.API.Responses;

namespace JamunaBank.Procurement.API.Authentication.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
