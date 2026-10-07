using JamunaBank.Procurement.API.Authentication;using JamunaBank.Procurement.API.DTOs;using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class DevelopmentEmployeeResolver(AuthenticationOptions options):IDevelopmentEmployeeResolver
{
 public DevelopmentEmployeeDto? FindByEmployeeId(string employeeId)
 {
  var user=options.DevelopmentUsers.FirstOrDefault(x=>string.Equals(x.EmployeeId,employeeId,StringComparison.OrdinalIgnoreCase));
 return user is null?null:new(user.EmployeeId,user.FullName,user.Email,user.Designation,user.OrgUnitId,user.OrgUnitCode);
 }

 public IReadOnlyList<DevelopmentEmployeeDto> Search(string? search)
 {
  var term=search?.Trim();
  return options.DevelopmentUsers
   .Where(x=>string.IsNullOrWhiteSpace(term)||x.EmployeeId.Contains(term,StringComparison.OrdinalIgnoreCase)||x.FullName.Contains(term,StringComparison.OrdinalIgnoreCase)||x.Email.Contains(term,StringComparison.OrdinalIgnoreCase)||x.OrgUnitCode.Contains(term,StringComparison.OrdinalIgnoreCase))
   .OrderBy(x=>x.FullName)
   .Take(20)
   .Select(x=>new DevelopmentEmployeeDto(x.EmployeeId,x.FullName,x.Email,x.Designation,x.OrgUnitId,x.OrgUnitCode))
   .ToArray();
 }
}
