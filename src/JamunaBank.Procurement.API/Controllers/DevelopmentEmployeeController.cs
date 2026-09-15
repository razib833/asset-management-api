using JamunaBank.Procurement.API.Constants;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JamunaBank.Procurement.API.Controllers;

[ApiController, Authorize(Policy = AuthorizationPolicyNames.Admin), Route("api/development-employees")]
public sealed class DevelopmentEmployeeController(IDevelopmentEmployeeResolver employees) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<IReadOnlyList<DevelopmentEmployeeDto>>> Search([FromQuery] string? search)
    {
        var result = employees.Search(search);
        return Ok(ApiResponse<IReadOnlyList<DevelopmentEmployeeDto>>.Ok(result, "Development employees loaded.", HttpContext.TraceIdentifier));
    }
}
