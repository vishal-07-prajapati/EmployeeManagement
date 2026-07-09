using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public decimal Salary { get; set; }
    }
}
