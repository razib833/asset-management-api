using JamunaBank.Procurement.API.DTOs;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IDevelopmentEmployeeResolver
{
    DevelopmentEmployeeDto? FindByEmployeeId(string employeeId);
    IReadOnlyList<DevelopmentEmployeeDto> Search(string? search);
}
