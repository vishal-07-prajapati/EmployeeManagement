using AutoMapper;
using EmployeeManagement.API.DTOs.Employee;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.API.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IMapper _mapper;
    public EmployeeService(
        IEmployeeRepository employeeRepository,
        ILogger<EmployeeService> logger,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
    }

    public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            return null;

        return _mapper.Map<EmployeeResponseDto>(employee);
    }

    public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        if (dto.Salary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        var employee = _mapper.Map<Models.Employee>(dto);

        employee.JoiningDate = DateTime.UtcNow;
        employee.IsActive = true;

        employee = await _employeeRepository.AddAsync(employee);

        return _mapper.Map<EmployeeResponseDto>(employee);
    }

    public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(dto.Id);

        if (employee == null)
            return false;

        _mapper.Map(dto, employee);

        await _employeeRepository.UpdateAsync(employee);

        return true;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            return false;

        await _employeeRepository.DeleteAsync(employee);

        return true;
    }
}