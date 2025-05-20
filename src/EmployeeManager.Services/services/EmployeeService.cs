using EmployeeManager.Repository.interfaces;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;

namespace EmployeeManager.Services.services;

public class EmployeeService : IEmployeeService
{
    private IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<GetAllEmployeeDto>> GetAllEmployees(CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllEmployees(cancellationToken);
        var employeeDtos = new List<GetAllEmployeeDto>();
        
        // mapping
        foreach (var employee in employees)
        {
            employeeDtos.Add(new GetAllEmployeeDto
            {
                Id = employee.Id,
                FullName = $"{employee.Person.FirstName} {employee.Person.MiddleName} {employee.Person.LastName}"
            });
        }

        return employeeDtos;
    }
}