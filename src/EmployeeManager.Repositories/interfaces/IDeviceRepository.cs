using EmployeeManager.API;

namespace EmployeeManager.Repository.interfaces;

public interface IDeviceRepository
{
    public Task<List<Device>> GetAllDevices(CancellationToken cancellationToken);
    
    public Task<Device?> GetDeviceById(int id, CancellationToken cancellationToken);
    
    public Task<bool> CreateDevice(Device device, CancellationToken cancellationToken);
    
    public Task<DeviceType?> GetDeviceTypeByName(string name, CancellationToken cancellationToken);

    public Task<bool> UpdateDevice(int id, Device updateDevice, CancellationToken cancellationToken);
    
    public Task<bool> DeleteDevice(int id, CancellationToken cancellationToken);
}