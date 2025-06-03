using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;
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

    [HttpGet]
    [Route("/api/devices")]
    public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
    {
        try
        {
            var devices = await _deviceService.GetAllDevices(cancellationToken);
            if (!devices.Any()) return NotFound("No devices found");
            return Ok(devices);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("/api/devices/{id}")]
    public async Task<IActionResult> GetDeviceById(int id, CancellationToken cancellationToken)
    {
        if (id < 0) return BadRequest("Invalid id");
        
        try
        {
            var device = await _deviceService.GetDeviceById(id, cancellationToken);
            if (device == null) return NotFound("Device not found");
            return Ok(device);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("/api/devices")]
    public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        try
        {
            await _deviceService.CreateDevice(dto, cancellationToken);
            return Created(string.Empty, null);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    [Route("/api/devices/{id}")]
    public async Task<IActionResult> UpdateDevice(int id, [FromBody] UpdateDeviceDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        if (id < 0) return BadRequest("Invalid id");
        
        try
        {
            await _deviceService.UpdateDevice(id, dto, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No device found with id: '{id}'");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    [Route("/api/devices/{id}")]
    public async Task<IActionResult> DeleteDevice(int id, CancellationToken cancellationToken)
    {
        if (id < 0) return BadRequest("Invalid id");
        
        try
        {
            await _deviceService.DeleteDevice(id, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No device found with id: '{id}'");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}