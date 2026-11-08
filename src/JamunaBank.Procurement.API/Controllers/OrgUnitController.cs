using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JamunaBank.Procurement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/org-units")]
public sealed class OrgUnitController(IOrgUnitService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<OrgUnitDto>>>> Get(
        [FromQuery] string? type = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetAsync(type, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<OrgUnitDto>>.Ok(result, "Organization units loaded.", HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<OrgUnitDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null
            ? NotFound(ApiResponse<OrgUnitDto>.Fail("Organization unit was not found.", ["No active organization unit has the specified ID."], HttpContext.TraceIdentifier))
            : Ok(ApiResponse<OrgUnitDto>.Ok(result, "Organization unit loaded.", HttpContext.TraceIdentifier));
    }
}
