using EmployeeManager.Services.context;
using EmployeeManager.Services.interfaces;
using EmployeeManager.Services.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EmployeeDatabase");
builder.Services.AddDbContext<EmployeeDatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();