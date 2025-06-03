using EmployeeManager.Models.models;
using EmployeeManager.Services.context;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.dtos.devices;
using EmployeeManager.Services.dtos.employees;
using EmployeeManager.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Services.services;

public class DeviceService : IDeviceService
{
    private EmployeeDatabaseContext _context;

    public DeviceService(EmployeeDatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<GetAllDeviceDto>> GetAllDevices(CancellationToken cancellationToken)
    {
        try
        {
            var deviceDtos = new List<GetAllDeviceDto>();
            var devices = await _context.Devices.ToListAsync(cancellationToken);
            // mapping to dtos
            foreach (var device in devices)
            {
                deviceDtos.Add(new GetAllDeviceDto
                {
                    Id = device.Id,
                    Name = device.Name
                });
            }

            return deviceDtos;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error retrieving devices", ex);
        }
    }

    public async Task<GetDeviceByIdDto?> GetDeviceById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var device = await _context.Devices
                .Include(d => d.DeviceType)
                .Include(de => de.DeviceEmployees)
                .ThenInclude(emp => emp.Employee)
                .ThenInclude(p => p.Person)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            
            if (device == null) return null;
        
            var deviceDto = new GetDeviceByIdDto
            {
                Name = device.Name,
                DeviceType = device.DeviceType.Name,
                AdditionalProperties = device.AdditionalProperties,
                CurrentEmployee = null
            };
        
            var currEmployee = device.DeviceEmployees.FirstOrDefault(e => e.ReturnDate == null);
            if (currEmployee != null)
            {
                deviceDto.CurrentEmployee = new GetAllEmployeeDto
                {
                    Id = currEmployee.Id,
                    FullName = $"{currEmployee.Employee.Person.FirstName} {currEmployee.Employee.Person.MiddleName} {currEmployee.Employee.Person.LastName}"
                };
            }
        
            return deviceDto;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error retrieving devices", ex);
        }
    }

    public async Task<bool> CreateDevice(CreateDeviceDto createDeviceDto, CancellationToken cancellationToken)
    {
        if (createDeviceDto.Name == null)
        {
            throw new ArgumentException("Invalid device name");
        }
        
        if (createDeviceDto.DeviceType == null)
        {
            throw new ArgumentException("Invalid device type");
        }
        
        try
        {
            var deviceType = await _context.DeviceTypes
                .FirstOrDefaultAsync(x => x.Name == createDeviceDto.DeviceType, cancellationToken);
            
            if (deviceType == null)
            {
                throw new ArgumentException("Invalid device type");
            }
        
            var device = new Device
            {
                Name = createDeviceDto.Name,
                DeviceType = deviceType,
                IsEnabled = createDeviceDto.IsEnabled,
                AdditionalProperties = (createDeviceDto.AdditionalProperties == null ? "" : createDeviceDto.AdditionalProperties).ToString()
            };

            await _context.Devices.AddAsync(device, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error creating a device.", ex);
        }
    }

    public async Task<bool> UpdateDevice(int id, UpdateDeviceDto updateDeviceDto, CancellationToken cancellationToken)
    {
        if (updateDeviceDto.Name == null)
        {
            throw new ArgumentException("Invalid device name");
        }

        if (updateDeviceDto.DeviceType == null)
        {
            throw new ArgumentException("Invalid device type");
        }

        try
        {
            var device = await _context.Devices
                .Include(d => d.DeviceType)
                .Include(de => de.DeviceEmployees)
                .ThenInclude(emp => emp.Employee)
                .ThenInclude(p => p.Person)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            
            if (device == null)
                throw new KeyNotFoundException("Device not found");

            var deviceType = await _context.DeviceTypes
                .FirstOrDefaultAsync(x => x.Name == updateDeviceDto.DeviceType, cancellationToken);
            
            if (deviceType == null)
            {
                throw new ArgumentException("Invalid device type");
            }
            
            var updateDevice = new Device
            {
                Name = updateDeviceDto.Name,
                IsEnabled = updateDeviceDto.IsEnabled,
                DeviceType = deviceType,
                AdditionalProperties = (updateDeviceDto.AdditionalProperties == null ? "" : updateDeviceDto.AdditionalProperties).ToString()
            };

            device.Name = updateDevice.Name;
            device.IsEnabled = updateDevice.IsEnabled;
            device.DeviceType = updateDevice.DeviceType;
            device.AdditionalProperties = updateDevice.AdditionalProperties;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error updating device with id {id}", ex);
        }
    }

    public async Task<bool> DeleteDevice(int id, CancellationToken cancellationToken)
    {
        var deviceCheck = await GetDeviceById(id, cancellationToken);
        if (deviceCheck == null) 
            throw new KeyNotFoundException("Device not found");
        
        try
        {
            _context.Devices.Remove(_context.Devices.FirstOrDefault(x => x.Id == id));
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error deleting device with id {id}", ex);
        }
    }
}