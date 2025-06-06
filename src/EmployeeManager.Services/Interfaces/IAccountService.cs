﻿using EmployeeManager.Services.dtos.accounts;
using EmployeeManager.Services.dtos.devices;

namespace EmployeeManager.Services.interfaces;

public interface IAccountService
{
    public Task<List<GetAllAccountsDto>> GetAllAccounts(CancellationToken cancellationToken);
    
    public Task<GetSpecificAccountDto> GetSpecificAccount(int id, CancellationToken cancellationToken);
    
    public Task<bool> CreateAccount(CreateAccountDto createAccountDto, CancellationToken cancellationToken);

    public Task<bool> UpdateAccount(int id, UpdateAccountDto updateAccountDto, CancellationToken cancellationToken);
    
    public Task<bool> DeleteAccount(int accountId, CancellationToken cancellationToken);
    
    public Task<ViewAccountDto> ViewAccount(string email, int id, CancellationToken cancellationToken);
    
    public Task<bool> UpdateUsersData(string email, int id, UpdateAccountDto updateDto, CancellationToken cancellationToken);
}