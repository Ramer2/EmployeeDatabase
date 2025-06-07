using EmployeeManager.Models.models;
using EmployeeManager.Services.context;
using EmployeeManager.Services.dtos.accounts;
using EmployeeManager.Services.dtos.devices;
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

    public async Task<List<GetAllAccountsDto>> GetAllAccounts(CancellationToken cancellationToken)
    {
        try
        {
            var accounts = await _context.Accounts
                .Include(account => account.Role)
                .Include(acc => acc.Employee)
                .ThenInclude(employee => employee.Person)
                .ToListAsync(cancellationToken);
            
            var accountsDto = new List<GetAllAccountsDto>();

            foreach (var acc in accounts)
            {
                accountsDto.Add(new GetAllAccountsDto
                {
                    Id = acc.Id,
                    Username = acc.Username,
                    Password = acc.Password,
                    RoleName = acc.Role.Name,
                    EmployeeFullName = $"{acc.Employee.Person.FirstName} {acc.Employee.Person.MiddleName} {acc.Employee.Person.LastName}"
                });
            }

            return accountsDto;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Problem getting all accounts", ex);
        }
    }

    public async Task<GetSpecificAccountDto> GetSpecificAccount(int id, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _context.Accounts
                .Include(acc => acc.Role)
                .Include(acc => acc.Employee)
                .ThenInclude(employee => employee.Person)
                .Where(acc => acc.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"Account with id {id} not found");
            
            return new GetSpecificAccountDto
            {
                Id = account.Id,
                Username = account.Username,
                Password = account.Password,
                RoleName = account.Role.Name,
                EmployeeFullName =
                    $"{account.Employee.Person.FirstName} {account.Employee.Person.MiddleName} {account.Employee.Person.LastName}"
            };
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Problem getting specific account", ex);
        }
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
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Problem creating account", ex);
        }
    }

    public async Task<bool> UpdateAccount(int id, UpdateAccountDto updateAccountDto, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _context.Employees
                .Include(emp => emp.Person)
                .Where(emp => emp.Person.Email == updateAccountDto.Email)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (employee == null)
                throw new KeyNotFoundException($"Employee with email {updateAccountDto.Email} does not exist.");
            
            var role = await _context.Roles
                .Where(r => r.Name == updateAccountDto.RoleName)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (role == null)
                throw new KeyNotFoundException($"No role with role {updateAccountDto.RoleName} exists.");

            var account = await _context.Accounts
                .Include(acc => acc.Employee)
                .ThenInclude(emp => emp.Person)
                .Where(acc => acc.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"No account with email {updateAccountDto.Email} exists.");
            
            account.Username = updateAccountDto.Username;
            account.Password = _passwordHasher.HashPassword(account, updateAccountDto.Password);
            account.Employee = employee;
            account.Role = role;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Problem updating account", ex);
        }
    }

    public async Task<bool> DeleteAccount(int accountId, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _context.Accounts
                .Where(a => a.Id == accountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"Account with id {accountId} does not exist.");
            
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Problem deleting account", ex);
        }
    }

    public async Task<ViewAccountDto> ViewAccount(string email, int id, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Accounts
                .Include(acc => acc.Employee)
                .ThenInclude(emp => emp.Person)
                .Where(acc => acc.Employee.Person.Email == email)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"No employee with email {email} exists.");

            if (user.Id != id)
                throw new AccessViolationException("User can have access only to their own account.");

            return new ViewAccountDto
            {
                Username = user.Username,
                Email = user.Employee.Person.Email,
                FullName =
                    $"{user.Employee.Person.FirstName} {user.Employee.Person.MiddleName} {user.Employee.Person.LastName}",
                PhoneNumber = user.Employee.Person.PhoneNumber,
                HireDate = user.Employee.HireDate.ToString(),
                PassportNumber = user.Employee.Person.PassportNumber
            };
        }
        catch (AccessViolationException)
        {
            throw;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while retrieving device data for the User", ex);
        }
    }

    // only for User
    public async Task<bool> UpdateUsersData(string email, int id, UpdateAccountDto updateDto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Accounts
                .Include(acc => acc.Role)
                .Include(acc => acc.Employee)
                .ThenInclude(emp => emp.Person)
                .Where(acc => acc.Employee.Person.Email == email)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (user == null)
                throw new KeyNotFoundException($"No user with email {email} exists.");
            
            if (user.Id != id)
                throw new AccessViolationException("User can have access only to their own account.");
            
            if (user.Role.Name != updateDto.RoleName)
                throw new AccessViolationException("User can't change their role.");

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name.Equals(updateDto.RoleName), cancellationToken);
            
            if (role == null)
                throw new KeyNotFoundException($"Role with name {updateDto.RoleName} does not exist.");
            
            user.Username = updateDto.Username;
            user.Password = _passwordHasher.HashPassword(user, updateDto.Password);
            user.Role = role;
            
            _context.Accounts.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (AccessViolationException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while updating personal data", ex);
        }
    }
}