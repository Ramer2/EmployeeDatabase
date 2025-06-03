using EmployeeManager.Models.models;
using EmployeeManager.Services.context;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Services.services;

public class AccountService : IAccountService
{
    private readonly PasswordHasher<Account> _passwordHasher = new();
    private readonly EmployeeDatabaseContext _context;

    public AccountService(EmployeeDatabaseContext context)
    {
        _context = context;
    }

    
    public async Task<bool> CreateAccount(CreateAccountDto createAccountDto, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _context.Employees
                .Where(emp => emp.Person.Email == createAccountDto.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (employee == null)
                throw new KeyNotFoundException($"No employee with email {createAccountDto.Email} exists.");
            
            var role = await _context.Roles
                .Where(r => r.Name == createAccountDto.RoleName)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (role == null)
                throw new KeyNotFoundException($"No role with role {createAccountDto.RoleName} exists.");

            var account = new Account
            {
                Username = createAccountDto.Username,
                Password = createAccountDto.Password,
                Employee = employee,
                Role = role
            };
            
            account.Password = _passwordHasher.HashPassword(account, createAccountDto.Password);
            
            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Problem creating account", ex);
        }
    }
}