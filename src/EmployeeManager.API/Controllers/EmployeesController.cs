using EmployeeManager.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.controllers;

[ApiController]
[Route("api/employees/[controller]")]
public class EmployeesController : ControllerBase
{
    private IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("/api/employees")]
    public async Task<IResult> GetAllEmployees(CancellationToken cancellationToken)
    {
        try
        {
            var employees = await _employeeService.GetAllEmployees(cancellationToken);
            if (employees.Count == 0) return Results.NotFound("No employees found");
            return Results.Ok(employees);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("/api/employees/{id}")]
    public async Task<IResult> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        if (id < 0) return Results.BadRequest("Invalid id");
        
        try
        {
            var employee = await _employeeService.GetEmployeeById(id, cancellationToken);
            if (employee == null) return Results.NotFound("Employee not found");
            return Results.Ok(employee);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}