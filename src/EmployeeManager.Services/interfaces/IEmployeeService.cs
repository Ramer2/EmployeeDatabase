using EmployeeManager.Services.dtos;

namespace EmployeeManager.Services.interfaces;

public interface IEmployeeService
{
    public Task<List<GetAllEmployeeDto>> GetAllEmployees(CancellationToken cancellationToken);
}