using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace JamunaBank.Procurement.API.Controllers;
[ApiController,Authorize,Route("api/asset-categories")]
public sealed class AssetCategoryController(IAssetCategoryService service):ControllerBase
{
    [HttpGet] public async Task<ActionResult<ApiResponse<IReadOnlyList<AssetCategoryDto>>>> Get([FromQuery]bool? isActive,CancellationToken ct){var x=await service.GetAllAsync(isActive,ct);return Ok(ApiResponse<IReadOnlyList<AssetCategoryDto>>.Ok(x,"Asset categories loaded.",HttpContext.TraceIdentifier));}
    [HttpGet("{id:long}")] public async Task<ActionResult<ApiResponse<AssetCategoryDto>>> GetById(long id,CancellationToken ct){var x=await service.GetByIdAsync(id,ct);return x is null?NotFound(ApiResponse<AssetCategoryDto>.Fail("Asset category was not found.",["No category has the specified ID."],HttpContext.TraceIdentifier)):Ok(ApiResponse<AssetCategoryDto>.Ok(x,"Asset category loaded.",HttpContext.TraceIdentifier));}
}
