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

    public async Task<GetEmployeeById?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeById(id, cancellationToken);
        if (employee == null) return null;

        var personDto = new PersonDto
        {
            Id = employee.Id,
            FirstName = employee.Person.FirstName,
            MiddleName = employee.Person.MiddleName,
            LastName = employee.Person.LastName,
            Email = employee.Person.Email,
            PassportNumber = employee.Person.PassportNumber,
            PhoneNumber = employee.Person.PhoneNumber
        };
        
        return new GetEmployeeById
        {
            PersonDto = personDto,
            PositionDto = new PositionDto { Id = employee.Position.Id, PositionName = employee.Position.Name },
            HireDate = employee.HireDate,
            Salary = employee.Salary
        };
    }
}