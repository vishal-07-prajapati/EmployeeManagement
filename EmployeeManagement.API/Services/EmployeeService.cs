using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new ArgumentException("Employee name is required.");

            if (employee.Salary < 0)
                throw new ArgumentException("Salary cannot be negative.");

            employee.JoiningDate = DateTime.UtcNow;
            employee.IsActive = true;

            return await _employeeRepository.AddAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.EmployeeId);

            if (existingEmployee == null)
                return false;

            existingEmployee.Name = employee.Name;
            existingEmployee.Department = employee.Department;
            existingEmployee.Email = employee.Email;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.IsActive = employee.IsActive;

            await _employeeRepository.UpdateAsync(existingEmployee);

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
}
