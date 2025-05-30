﻿using EmployeeManager.API;
using EmployeeManager.Repository.context;
using EmployeeManager.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Repository.repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private EmployeeDatabaseContext _context;

    public EmployeeRepository(EmployeeDatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Employees
                .Include(p => p.Person)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while getting all employees", ex);
        }
    }

    public Task<Employee?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        try
        {
            return _context.Employees
                .Include(p => p.Person)
                .Include(pos => pos.Position)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while getting employee by id", ex);
        }
    }
}