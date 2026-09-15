using JamunaBank.Procurement.API.Models;
using Microsoft.AspNetCore.Mvc;
using JamunaBank.Procurement.API.Services.Interfaces;

namespace JamunaBank.Procurement.API.Controllers;

[ApiController]
[Route("api/system")]
public sealed class SystemController(IHostEnvironment environment, IDatabaseHealthService databaseHealth) : ControllerBase
{
    [HttpGet("health")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<object>> Health()
    {
        var data = new { Status = "Healthy", Environment = environment.EnvironmentName };
        return Ok(ApiResponse<object>.Ok(data, "API foundation is running.", HttpContext.TraceIdentifier));
    }

    [HttpGet("database-health")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> DatabaseHealth(CancellationToken cancellationToken)
    {
        _ = await databaseHealth.CountActiveOrganizationUnitsAsync(cancellationToken);
        var data = new { Status = "Healthy" };
        return Ok(ApiResponse<object>.Ok(data, "Stored-procedure database access succeeded.", HttpContext.TraceIdentifier));
    }
}
