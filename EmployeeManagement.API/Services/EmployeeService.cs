using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.API.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        _logger.LogInformation("Fetching all employees.");

        return await _employeeRepository.GetAllAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        _logger.LogInformation("Fetching employee with Id: {EmployeeId}", id);

        return await _employeeRepository.GetByIdAsync(id);
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        _logger.LogInformation("Creating employee: {EmployeeName}", employee.Name);

        if (string.IsNullOrWhiteSpace(employee.Name))
            throw new ArgumentException("Employee name is required.");

        if (employee.Salary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        employee.JoiningDate = DateTime.UtcNow;
        employee.IsActive = true;

        var createdEmployee = await _employeeRepository.AddAsync(employee);

        _logger.LogInformation(
            "Employee created successfully with Id: {EmployeeId}",
            createdEmployee.EmployeeId);

        return createdEmployee;
    }

    public async Task<bool> UpdateEmployeeAsync(Employee employee)
    {
        _logger.LogInformation("Updating employee: {EmployeeId}", employee.EmployeeId);

        var existingEmployee = await _employeeRepository.GetByIdAsync(employee.EmployeeId);

        if (existingEmployee == null)
        {
            _logger.LogWarning(
                "Employee not found with Id: {EmployeeId}",
                employee.EmployeeId);

            return false;
        }

        existingEmployee.Name = employee.Name;
        existingEmployee.Department = employee.Department;
        existingEmployee.Email = employee.Email;
        existingEmployee.Salary = employee.Salary;
        existingEmployee.IsActive = employee.IsActive;

        await _employeeRepository.UpdateAsync(existingEmployee);

        _logger.LogInformation(
            "Employee updated successfully: {EmployeeId}",
            employee.EmployeeId);

        return true;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        _logger.LogInformation("Deleting employee: {EmployeeId}", id);

        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            _logger.LogWarning(
                "Employee not found with Id: {EmployeeId}",
                id);

            return false;
        }

        await _employeeRepository.DeleteAsync(employee);

        _logger.LogInformation(
            "Employee deleted successfully: {EmployeeId}",
            id);

        return true;
    }
}