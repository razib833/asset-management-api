using JamunaBank.Procurement.API.Constants;using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace JamunaBank.Procurement.API.Controllers;
[ApiController,Authorize(Policy=AuthorizationPolicyNames.Admin),Route("api/procurement/dashboard")]
public sealed class ProcurementDashboardController(IProcurementDashboardService service):ControllerBase
{
 [HttpGet]public async Task<ActionResult<ApiResponse<ProcurementDashboardDto>>>Get([FromQuery]int?year,CancellationToken ct){var selected=year??DateTime.Today.Year;var result=await service.GetAsync(selected,ct);return Ok(ApiResponse<ProcurementDashboardDto>.Ok(result,"Procurement dashboard loaded.",HttpContext.TraceIdentifier));}
 [HttpGet("requisitions")]public async Task<ActionResult<ApiResponse<IReadOnlyList<BranchRequisitionDto>>>>Requisitions([FromQuery]string?requisitionNo,[FromQuery]long?assetId,[FromQuery]string?requisitionType,[FromQuery]string?status,[FromQuery]DateOnly?fromDate,[FromQuery]DateOnly?toDate,CancellationToken ct){var x=await service.GetRequisitionsAsync(requisitionNo,assetId,requisitionType,status,fromDate,toDate,ct);return Ok(ApiResponse<IReadOnlyList<BranchRequisitionDto>>.Ok(x,"Management requisitions loaded.",HttpContext.TraceIdentifier));}
 [HttpGet("requisitions/{id:long}")]public async Task<ActionResult<ApiResponse<RequisitionDetailDto>>>Requisition(long id,CancellationToken ct){var x=await service.GetRequisitionAsync(id,ct);return x is null?NotFound(ApiResponse<RequisitionDetailDto>.Fail("Requisition was not found.",[],HttpContext.TraceIdentifier)):Ok(ApiResponse<RequisitionDetailDto>.Ok(x,"Requisition loaded.",HttpContext.TraceIdentifier));}
}
