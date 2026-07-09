using AutoMapper;
using EmployeeManagement.API.DTOs.Employee;
using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<CreateEmployeeDto, Employee>();

        CreateMap<UpdateEmployeeDto, Employee>();

        CreateMap<Employee, EmployeeResponseDto>();
    }
}