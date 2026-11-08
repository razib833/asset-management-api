using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Authentication.Interfaces;

public interface IAuthenticationProvider
{
    Task<ApplicationUser?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);
}
