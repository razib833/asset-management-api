using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ProcurementOfficerAssetMappingDto(long MappingId,long AssetId,string EmployeeId,string? FullName,string? Email,string? Designation,bool IsPrimary,DateOnly EffectiveFrom,DateOnly? EffectiveTo,bool IsActive);
public sealed class AddProcurementOfficerAssetMappingRequest
{
 [Required,StringLength(30)] public string EmployeeId{get;init;}=string.Empty;
 public bool IsPrimary{get;init;}
 public DateOnly EffectiveFrom{get;init;}=DateOnly.FromDateTime(DateTime.UtcNow);
 public DateOnly? EffectiveTo{get;init;}
}
