using JamunaBank.Procurement.API.Constants;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace JamunaBank.Procurement.API.Controllers;
[ApiController,Authorize,Route("api/assets")]
public sealed class AssetController(IAssetService service):ControllerBase
{
    [HttpGet] public async Task<ActionResult<ApiResponse<IReadOnlyList<AssetDto>>>> Search([FromQuery]long? assetCategoryId,[FromQuery]string? assetName,[FromQuery]string? assetCode,[FromQuery]bool? isActive,CancellationToken ct){var x=await service.SearchAsync(assetCategoryId,assetName,assetCode,isActive,ct);return Ok(ApiResponse<IReadOnlyList<AssetDto>>.Ok(x,"Assets loaded.",HttpContext.TraceIdentifier));}
    [HttpGet("{id:long}")] public async Task<ActionResult<ApiResponse<AssetDto>>> GetById(long id,CancellationToken ct){var x=await service.GetByIdAsync(id,ct);return x is null?NotFound(ApiResponse<AssetDto>.Fail("Asset was not found.",["No asset has the specified ID."],HttpContext.TraceIdentifier)):Ok(ApiResponse<AssetDto>.Ok(x,"Asset loaded.",HttpContext.TraceIdentifier));}
    [HttpPost,Authorize(Policy=AuthorizationPolicyNames.Admin)] public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> Create(CreateAssetRequest request,CancellationToken ct)=>Map(await service.CreateAsync(request,ct),"Asset created.");
    [HttpPut("{id:long}"),Authorize(Policy=AuthorizationPolicyNames.Admin)] public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> Update(long id,UpdateAssetRequest request,CancellationToken ct)=>Map(await service.UpdateAsync(id,request,ct),"Asset updated.");
    [HttpPut("{id:long}/activation"),Authorize(Policy=AuthorizationPolicyNames.Admin)] public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> SetActive(long id,SetActiveRequest request,CancellationToken ct)=>Map(await service.SetActiveAsync(id,request.IsActive,ct),request.IsActive?"Asset activated.":"Asset deactivated.");
    private ActionResult<ApiResponse<StoredProcedureResult>> Map(StoredProcedureResult? result,string message)
    {
        if(result is null)return StatusCode(500,ApiResponse<StoredProcedureResult>.Fail("The stored procedure returned no result.",["No operation result was returned."],HttpContext.TraceIdentifier));
        return result.ResultCode switch{ResultCodes.Success=>Ok(ApiResponse<StoredProcedureResult>.Ok(result,message,HttpContext.TraceIdentifier)),ResultCodes.NotFound=>NotFound(ApiResponse<StoredProcedureResult>.Fail("Asset was not found.",[result.ResultMessage],HttpContext.TraceIdentifier)),ResultCodes.Conflict=>Conflict(ApiResponse<StoredProcedureResult>.Fail("Asset code already exists.",[result.ResultMessage],HttpContext.TraceIdentifier)),_=>StatusCode(500,ApiResponse<StoredProcedureResult>.Fail("Asset operation failed.",[result.ResultMessage],HttpContext.TraceIdentifier))};
    }
}
