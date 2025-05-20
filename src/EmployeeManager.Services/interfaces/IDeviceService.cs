using EmployeeManager.Services.dtos;

namespace EmployeeManager.Services.interfaces;

public interface IDeviceService
{
    public Task<List<GetAllDeviceDto>> GetAllDevices(CancellationToken cancellationToken);
    
    public Task<GetDeviceByIdDto?> GetDeviceById(int id, CancellationToken cancellationToken);
    
    public Task<bool> CreateDevice(CreateDeviceDto createDeviceDto, CancellationToken cancellationToken);
}