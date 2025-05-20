using EmployeeManager.Repository.context;
using EmployeeManager.Repository.interfaces;
using EmployeeManager.Repository.repositories;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;
using EmployeeManager.Services.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EmployeeDatabase");
builder.Services.AddDbContext<EmployeeDatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddTransient<IDeviceService, DeviceService>();

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

app.MapGet("/api/employees", async (EmployeeDatabaseContext context, CancellationToken cancellationToken) =>
{
    throw new NotImplementedException();
});

app.MapGet("/api/employees/{id}", async (EmployeeDatabaseContext context, CancellationToken cancellationToken, int id) =>
{
    throw new NotImplementedException();
});

app.Run();
