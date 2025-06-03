using EmployeeManager.Services.dtos;

namespace EmployeeManager.Services.interfaces;

public interface IAccountService
{
    public Task<bool> CreateAccount(CreateAccountDto createAccountDto, CancellationToken cancellationToken);
}