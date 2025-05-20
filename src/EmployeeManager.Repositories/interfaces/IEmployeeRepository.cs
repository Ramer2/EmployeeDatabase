using EmployeeManager.API;

namespace EmployeeManager.Repository.interfaces;

public interface IEmployeeRepository
{
    public Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken);
    
    public Task<Employee?> GetEmployeeById(int id, CancellationToken cancellationToken);
}