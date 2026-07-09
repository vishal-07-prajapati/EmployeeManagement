using EmployeeManagement.API.DTOs.Employee;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region DI
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();

            var response = employees.Select(employee => new EmployeeResponseDto
            {
                Id = employee.EmployeeId,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                Salary = employee.Salary,
                IsActive = employee.IsActive
            });

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound();

            var response = new EmployeeResponseDto
            {
                Id = employee.EmployeeId,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                Salary = employee.Salary,
                IsActive = employee.IsActive
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Department = dto.Department,
                Email = dto.Email,
                Salary = dto.Salary
            };

            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee);

            var response = new EmployeeResponseDto
            {
                Id = createdEmployee.EmployeeId,
                Name = createdEmployee.Name,
                Department = createdEmployee.Department,
                Email = createdEmployee.Email,
                Salary = createdEmployee.Salary,
                IsActive = createdEmployee.IsActive
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Route Id and Body Id must match.");

            var employee = new Employee
            {
                EmployeeId = dto.Id,
                Name = dto.Name,
                Department = dto.Department,
                Email = dto.Email,
                Salary = dto.Salary,
                IsActive = dto.IsActive
            };

            var updated = await _employeeService.UpdateEmployeeAsync(employee);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
