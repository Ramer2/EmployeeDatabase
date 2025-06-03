using EmployeeManager.Services.interfaces;
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

    [HttpGet]
    [Route("/api/employees")]
    public async Task<IActionResult> GetAllEmployees(CancellationToken cancellationToken)
    {
        try
        {
            var employees = await _employeeService.GetAllEmployees(cancellationToken);
            if (employees.Count == 0) return NotFound("No employees found");
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("/api/employees/{id}")]
    public async Task<IActionResult> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        if (id < 0) return BadRequest("Invalid id");
        
        try
        {
            var employee = await _employeeService.GetEmployeeById(id, cancellationToken);
            if (employee == null) return NotFound("Employee not found");
            return Ok(employee);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}