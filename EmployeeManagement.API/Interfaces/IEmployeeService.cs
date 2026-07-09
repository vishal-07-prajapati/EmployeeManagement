using EmployeeManagement.API.DTOs.Employee;

namespace EmployeeManagement.API.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();

    Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);

    Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDto dto);

    Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto);

    Task<bool> DeleteEmployeeAsync(int id);
}