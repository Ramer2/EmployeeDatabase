using EmployeeManager.Services.context;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Services.services;

public class EmployeeService : IEmployeeService
{
    private EmployeeDatabaseContext _context;

    public EmployeeService(EmployeeDatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<GetAllEmployeeDto>> GetAllEmployees(CancellationToken cancellationToken)
    {
        try
        {
            var employees = await _context.Employees
                .Include(p => p.Person)
                .ToListAsync();
            
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
        catch (Exception ex)
        {
            throw new ApplicationException("Error while getting all employees", ex);
        }
    }

    public async Task<GetEmployeeById?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _context.Employees
                .Include(p => p.Person)
                .Include(pos => pos.Position)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            
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
        catch (Exception ex)
        {
            throw new ApplicationException("Error while getting employee by id", ex);
        }
    }
}