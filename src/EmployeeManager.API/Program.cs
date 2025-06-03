using EmployeeManager.Repository.context;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;
using EmployeeManager.Services.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EmployeeDatabase");
builder.Services.AddDbContext<EmployeeDatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/devices", async (IDeviceService deviceService, CancellationToken cancellationToken) =>
{
    try
    {
        var devices = await deviceService.GetAllDevices(cancellationToken);
        if (devices.ToList().Count == 0) return Results.NotFound("No devices found");

        return Results.Ok(devices);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/devices/{id}", async (IDeviceService deviceService, CancellationToken cancellationToken, int id) =>
{
    try
    {
        var device = await deviceService.GetDeviceById(id, cancellationToken);
        if (device == null) return Results.NotFound("Device not found");
        
        return Results.Ok(device);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/devices", async (IDeviceService deviceService, CancellationToken cancellationToken, CreateDeviceDto createDeviceDto) =>
{
    try
    {
        await deviceService.CreateDevice(createDeviceDto, cancellationToken);
        return Results.Created();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPut("/api/devices/{id}", async (int id, UpdateDeviceDto updateDeviceDto, IDeviceService deviceService, CancellationToken cancellationToken) =>
{
    try
    {
        await deviceService.UpdateDevice(id, updateDeviceDto, cancellationToken);
        return Results.Ok();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No device found with id: '{id}'");
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/api/devices/{id}", async (int id, IDeviceService deviceService, CancellationToken cancellationToken) =>
{
    try
    {
        await deviceService.DeleteDevice(id, cancellationToken);
        return Results.Ok();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No device found with id: '{id}'");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/employees", async (IEmployeeService employeeService, CancellationToken cancellationToken) =>
{
    try
    {
        var employees = await employeeService.GetAllEmployees(cancellationToken);
        if (employees.Count == 0) return Results.NotFound("No employees found");

        return Results.Ok(employees);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/employees/{id}", async (int id, IEmployeeService employeeService, CancellationToken cancellationToken) =>
{
    try
    {
        var employee = await employeeService.GetEmployeeById(id, cancellationToken);
        if (employee == null) return Results.NotFound("Employee not found");
        
        return Results.Ok(employee);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
