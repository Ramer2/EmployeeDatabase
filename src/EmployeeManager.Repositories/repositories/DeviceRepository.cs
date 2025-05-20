using EmployeeManager.API;
using EmployeeManager.Repository.context;
using EmployeeManager.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Repository.repositories;

public class DeviceRepository : IDeviceRepository
{
    private EmployeeDatabaseContext _context;

    public DeviceRepository(EmployeeDatabaseContext context)
    {
        _context = context;
    }

    public Task<List<Device>> GetAllDevices(CancellationToken cancellationToken)
    {
        try
        {
            return _context.Devices.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error retrieving devices", ex);
        }
    }

    public Task<Device?> GetDeviceById(int id, CancellationToken cancellationToken)
    {
        try
        {
            return _context.Devices
                .Include(d => d.DeviceType)
                .Include(de => de.DeviceEmployees)
                .ThenInclude(emp => emp.Employee)
                .ThenInclude(p => p.Person)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error retrieving device with id {id}", ex);
        }
    }

    public async Task<bool> CreateDevice(Device device, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Devices.AddAsync(device, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error creating a device.", ex);
        }
    }

    public async Task<DeviceType?> GetDeviceTypeByName(string name, CancellationToken cancellationToken)
    {
        try
        {
            return await _context.DeviceTypes.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error retrieving device type with name {name}", ex);
        }
    }

    public async Task<bool> UpdateDevice(int id, Device updateDevice, CancellationToken cancellationToken)
    {
        try
        {
            var device = await GetDeviceById(id, cancellationToken);

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
}