using JamunaBank.Procurement.API.Models;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public sealed record AttachmentFileContent(Stream Stream,string ContentType,string FileName);
public interface IAttachmentFileService
{
 Task<StoredProcedureResult?> UploadAsync(string entityType,long entityId,IFormFile file,CancellationToken ct);
 Task<AttachmentFileContent?> OpenAsync(long attachmentId,string entityType,long entityId,CancellationToken ct);
}
