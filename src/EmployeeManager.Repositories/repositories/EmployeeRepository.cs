using EmployeeManager.API;
using EmployeeManager.Repository.context;
using EmployeeManager.Repository.interfaces;

namespace EmployeeManager.Repository.repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private EmployeeDatabaseContext _context;

    public EmployeeRepository(EmployeeDatabaseContext context)
    {
        _context = context;
    }
}