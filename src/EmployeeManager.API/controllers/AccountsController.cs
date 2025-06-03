using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;
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
