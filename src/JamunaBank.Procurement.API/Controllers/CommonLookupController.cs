using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JamunaBank.Procurement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/common-lookups")]
public sealed class CommonLookupController(ICommonLookupService service) : ControllerBase
{
    [HttpGet("{category}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CommonLookupDto>>>> GetByCategory(
        string category,
        CancellationToken cancellationToken)
    {
        var result = await service.GetByCategoryAsync(category, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<CommonLookupDto>>.Ok(result, "Lookup values loaded.", HttpContext.TraceIdentifier));
    }
}
