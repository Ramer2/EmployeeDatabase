using System.Security.Claims;
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

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    [Route("/api/accounts")]
    public async Task<IResult> GetAccounts(CancellationToken cancellationToken)
    {
        try
        {
            if (User.IsInRole("Admin"))
            {
                var accounts = await _accountService.GetAllAccounts(cancellationToken);
                if (accounts.Count == 0)
                    return Results.NotFound();

                return Results.Ok(accounts);                
            }
            else
            { 
                if (User.FindFirst(ClaimTypes.Email) == null) 
                    return Results.Problem("Invalid credentials");
                
                var res = await _accountService
                    .ViewAccount(User.FindFirst(ClaimTypes.Email).Value, cancellationToken);
                
                return Results.Ok(res);
            }
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("/api/accounts/{id}")]
    public async Task<IResult> GetAccountById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _accountService.GetSpecificAccount(id, cancellationToken);
            return Results.Ok(account);
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
            return Results.Created();
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

    [Authorize(Roles = "Admin")]
    [HttpPut]
    [Route("/api/accounts/{id}")]
    public async Task<IResult> UpdateAccount(int id, [FromBody] UpdateAccountDto account, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            await _accountService.UpdateAccount(id, account, cancellationToken);
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

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("/api/accounts/{id}")]
    public async Task<IResult> DeleteAccount(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _accountService.DeleteAccount(id, cancellationToken);
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

    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("/api/accounts/devices")]
    public async Task<IResult> GetAssignedDevices(CancellationToken cancellationToken)
    {
        try
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (email == null) 
                return Results.Problem("Invalid credentials");

            var devices = await _accountService.ViewAssignedDevices(email, cancellationToken);
            return Results.Ok(devices);
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
