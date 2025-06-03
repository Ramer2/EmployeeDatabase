using EmployeeManager.Services.dtos;
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
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            
            
            
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
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
