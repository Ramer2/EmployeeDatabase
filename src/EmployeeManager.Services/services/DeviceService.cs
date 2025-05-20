using System.Text.Json;
using EmployeeManager.API;
using EmployeeManager.Repository.interfaces;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;

namespace EmployeeManager.Services.services;

public class DeviceService : IDeviceService
{
    private IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<List<GetAllDeviceDto>> GetAllDevices(CancellationToken cancellationToken)
    {
        var deviceDtos = new List<GetAllDeviceDto>();
        var devices = await _deviceRepository.GetAllDevices(cancellationToken);

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

    public async Task<GetDeviceByIdDto?> GetDeviceById(int id, CancellationToken cancellationToken)
    {
        var device = await _deviceRepository.GetDeviceById(id, cancellationToken);
        if (device == null) return null;
        
        var deviceDto = new GetDeviceByIdDto
        {
            Name = device.Name,
            DeviceType = device.DeviceType.Name,
            AdditionalProperties = JsonDocument.Parse(device.AdditionalProperties).RootElement,
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

    public async Task<bool> CreateDevice(CreateDeviceDto createDeviceDto, CancellationToken cancellationToken)
    {
        if (createDeviceDto.DeviceType == null)
        {
            throw new ArgumentException("Invalid device type");
        }

        if (createDeviceDto.Name == null)
        {
            throw new ArgumentException("Invalid device name");
        }
        
        var deviceType = await _deviceRepository.GetDeviceTypeByName(createDeviceDto.DeviceType, cancellationToken);
        if (deviceType == null)
        {
            throw new ArgumentException("Invalid device type");
        }
        
        var device = new Device
        {
            Name = createDeviceDto.Name,
            DeviceType = deviceType,
            IsEnabled = createDeviceDto.IsEnabled,
            AdditionalProperties = createDeviceDto.AdditionalProperties == null ? "" : createDeviceDto.AdditionalProperties
        };

        await _deviceRepository.CreateDevice(device, cancellationToken);
        return true;
    }

    public async Task<bool> UpdateDevice(int id, UpdateDeviceDto updateDeviceDto, CancellationToken cancellationToken)
    {
        if (updateDeviceDto.DeviceType == null)
        {
            throw new ArgumentException("Invalid device type");
        }
        
        if (updateDeviceDto.Name == null)
        {
            throw new ArgumentException("Invalid device name");
        }
        
        var deviceCheck = await _deviceRepository.GetDeviceById(id, cancellationToken);
        if (deviceCheck == null) 
            throw new KeyNotFoundException("Device not found");
        
        var deviceType = await _deviceRepository.GetDeviceTypeByName(updateDeviceDto.DeviceType, cancellationToken);
        if (deviceType == null)
        {
            throw new ArgumentException("Invalid device type");
        }
        
        var device = new Device
        {
            Name = updateDeviceDto.Name,
            IsEnabled = updateDeviceDto.IsEnabled,
            DeviceType = deviceType,
            AdditionalProperties = updateDeviceDto.AdditionalProperties == null ? "" : updateDeviceDto.AdditionalProperties
        };
        return await _deviceRepository.UpdateDevice(id, device, cancellationToken);
    }

    public async Task<bool> DeleteDevice(int id, CancellationToken cancellationToken)
    {
        var deviceCheck = await _deviceRepository.GetDeviceById(id, cancellationToken);
        if (deviceCheck == null) 
            throw new KeyNotFoundException("Device not found");
        
        return await _deviceRepository.DeleteDevice(id, cancellationToken);
    }
}