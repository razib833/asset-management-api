using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record ConcernDivisionDto(long Id,long OrgUnitId,string SetupCode,bool IsActive,string OrgUnitCode,string OrgUnitName,string OrgUnitType);
public sealed record ConcernOfficialDto(long MappingId,long ConcernDivisionId,string EmployeeId,string? FullName,string? Email,string? Designation,bool IsActive,DateOnly EffectiveFrom,DateOnly? EffectiveTo);
public sealed record ConcernAuthorityDto(long MappingId,long ConcernDivisionId,string EmployeeId,string? FullName,string? Email,string? Designation,int AuthorityLevel,bool IsActive,DateOnly EffectiveFrom,DateOnly? EffectiveTo);
public sealed record DevelopmentEmployeeDto(string EmployeeId,string FullName,string Email,string Designation,long OrgUnitId,string OrgUnitCode);
public class AddConcernOfficialRequest
{
 [Required,StringLength(30)] public string EmployeeId{get;init;}=string.Empty;
 public DateOnly EffectiveFrom{get;init;}=DateOnly.FromDateTime(DateTime.UtcNow);
 public DateOnly? EffectiveTo{get;init;}
}
public sealed class AddConcernAuthorityRequest:AddConcernOfficialRequest
{
 [Range(1,int.MaxValue)] public int AuthorityLevel{get;init;}=1;
}
