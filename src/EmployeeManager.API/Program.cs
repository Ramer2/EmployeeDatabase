using EmployeeManager.Repository.context;
using EmployeeManager.Services.dtos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EmployeeDatabase");
builder.Services.AddDbContext<EmployeeDatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/devices", async (EmployeeDatabaseContext context, CancellationToken cancellationToken) =>
{
    throw new NotImplementedException();
});

app.MapGet("/api/devices/{id}", async (EmployeeDatabaseContext context, CancellationToken cancellationToken, string id) =>
{
    throw new NotImplementedException();
});

app.MapPost("/api/devices", async (EmployeeDatabaseContext context, CancellationToken cancellationToken, CreateDeviceDto createDeviceDto) =>
{
    throw new NotImplementedException();
});

app.MapPut("/api/devices/{id}", async (EmployeeDatabaseContext context, CancellationToken cancellationToken, string id, CreateDeviceDto createDeviceDto) =>
{
    throw new NotImplementedException();
});

app.MapDelete("/api/devices/{id}", async (EmployeeDatabaseContext context, CancellationToken cancellationToken, string id) =>
{
    throw new NotImplementedException();
});

app.MapGet("/api/employees", async (EmployeeDatabaseContext context, CancellationToken cancellationToken) =>
{
    throw new NotImplementedException();
});

app.MapGet("/api/employees", async (EmployeeDatabaseContext context, CancellationToken cancellationToken) =>
{
    throw new NotImplementedException();
});

app.Run();
