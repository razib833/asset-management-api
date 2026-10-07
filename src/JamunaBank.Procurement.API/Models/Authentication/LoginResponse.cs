using JamunaBank.Procurement.API.Models;

namespace JamunaBank.Procurement.API.Responses;

public sealed record LoginResponse(string AccessToken, DateTimeOffset ExpiresAt, ApplicationUser User);
