using EmployeeManager.Services.dtos.accounts;
using EmployeeManager.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.controllers;

[Route("api/accounts/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    
    private readonly IAccountService _accountService;
    
    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("/api/accounts")]
    public async Task<IResult> GetAccounts(CancellationToken cancellationToken)
    {
        try
        {
            var accounts = await _accountService.GetAllAccounts(cancellationToken);
            if (accounts.Count == 0)
                return Results.NotFound();

            return Results.Ok(accounts);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("/api/accounts")]
    public async Task<IResult> CreateAccount([FromBody] CreateAccountDto newAccount, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            await _accountService.CreateAccount(newAccount, cancellationToken);
            return Results.Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
