using EmployeeManager.Services.dtos.devices;
using EmployeeManager.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.controllers;

[ApiController]
[Route("api/devices/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("/api/devices")]
    public async Task<IResult> GetAllDevices(CancellationToken cancellationToken)
    {
        try
        {
            var devices = await _deviceService.GetAllDevices(cancellationToken);
            if (!devices.Any()) return Results.NotFound("No devices found");
            return Results.Ok(devices);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("/api/devices/{id}")]
    public async Task<IResult> GetDeviceById(int id, CancellationToken cancellationToken)
    {
        if (id < 0) return Results.BadRequest("Invalid id");
        
        try
        {
            var device = await _deviceService.GetDeviceById(id, cancellationToken);
            if (device == null) return Results.NotFound("Device not found");
            return Results.Ok(device);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("/api/devices")]
    public async Task<IResult> CreateDevice([FromBody] CreateDeviceDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return Results.BadRequest(ModelState);
        
        try
        {
            await _deviceService.CreateDevice(dto, cancellationToken);
            return Results.Created(string.Empty, null);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    [Route("/api/devices/{id}")]
    public async Task<IResult> UpdateDevice(int id, [FromBody] UpdateDeviceDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return Results.BadRequest(ModelState);
        
        if (id < 0) return Results.BadRequest("Invalid id");
        
        try
        {
            await _deviceService.UpdateDevice(id, dto, cancellationToken);
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
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("/api/devices/{id}")]
    public async Task<IResult> DeleteDevice(int id, CancellationToken cancellationToken)
    {
        if (id < 0) return Results.BadRequest("Invalid id");
        
        try
        {
            await _deviceService.DeleteDevice(id, cancellationToken);
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
    }
}