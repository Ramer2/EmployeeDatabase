using EmployeeManager.Services.dtos.accounts;

namespace EmployeeManager.Services.interfaces;

public interface IAccountService
{
    public Task<bool> CreateAccount(CreateAccountDto createAccountDto, CancellationToken cancellationToken);
    
    public Task<List<GetAllAccountsDto>> GetAllAccounts(CancellationToken cancellationToken);
}