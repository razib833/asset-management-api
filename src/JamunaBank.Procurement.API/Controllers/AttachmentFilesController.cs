using JamunaBank.Procurement.API.Constants;using JamunaBank.Procurement.API.Models;using JamunaBank.Procurement.API.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace JamunaBank.Procurement.API.Controllers;
[ApiController,Authorize,Route("api/attachments")]
public sealed class AttachmentFilesController(IAttachmentFileService service):ControllerBase
{
 [HttpPost("upload")][RequestSizeLimit(10*1024*1024)]public async Task<ActionResult<ApiResponse<StoredProcedureResult>>>Upload([FromForm]string entityType,[FromForm]long entityId,IFormFile file,CancellationToken ct){var r=await service.UploadAsync(entityType,entityId,file,ct);return r?.ResultCode==ResultCodes.Success?Ok(ApiResponse<StoredProcedureResult>.Ok(r,"Attachment uploaded.",HttpContext.TraceIdentifier)):BadRequest(ApiResponse<StoredProcedureResult>.Fail("Attachment upload failed.",[r?.ResultMessage??"No result returned."],HttpContext.TraceIdentifier));}
 [HttpGet("{attachmentId:long}/content")]public async Task<IActionResult>Content(long attachmentId,[FromQuery]string entityType,[FromQuery]long entityId,CancellationToken ct){var file=await service.OpenAsync(attachmentId,entityType,entityId,ct);return file is null?NotFound():File(file.Stream,file.ContentType,file.FileName,enableRangeProcessing:true);}
}
